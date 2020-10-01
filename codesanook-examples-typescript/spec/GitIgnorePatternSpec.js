const parser = require('gitignore-parser');

describe('git ignore /* pattern', () => {
    /*
        Credit https://stackoverflow.com/a/38559600/1872200
        /dir will match a file, directory, link, anything named dir
        /dir/ will match only a directory named dir
        /dir/* will match all files, directories and anything else inside a directory named dir (but not the dir directory itself).
    */
    it('should ignore file and direcotry', () => {

        // const gitIgnoreContent = fs.readFileSync('.gitignore', 'utf8');
        const gitIgnoreContent = [
            // Match a file, directory, link or anything named dir
            '/dir',
        ].join('\n');

        const gitignore = parser.compile(gitIgnoreContent);
        const files = [
            '/dir', // a file
            '/dir/', // a directory
            '/dir/test.txt',
            '/dir/child/test.txt',
        ];

        // ignore file named dir, dir directory and files under direcotry   
        const denies = files.filter(gitignore.denies);
        expect(denies.length).toBe(4);

        const accepts = files.filter(gitignore.accepts);
        expect(accepts.length).toBe(0);
    });

    it('should ignore directory only', () => {
        const gitIgnoreContent = [
            // Match only a directory named dir
            '/dir/',
        ].join('\n');

        const gitignore = parser.compile(gitIgnoreContent);
        const files = [
            '/dir', // a file
            '/dir/', // a directory
            '/dir/test.txt',
            '/dir/child/test.txt',
        ];

        const denies = files.filter(gitignore.denies);
        expect(denies.length).toBe(3);

        // Not match a file named dir
        const accepts = files.filter(gitignore.accepts);
        expect(accepts.length).toBe(1);
        expect(accepts[0]).toBe('/dir');
    });

    it('should ignore files/directories under the current directory', () => {
        const gitIgnoreContent = [
            // Match all files, directories and anything else inside a directory named dir (but not the dir directory itself).
            '/dir/*',
        ].join('\n');

        const gitignore = parser.compile(gitIgnoreContent);

        const files = [
            '/dir',// a file
            '/dir/', // a directory
            '/dir/test.txt',
            '/dir/child/test.txt',
        ];
        const denies = files.filter(gitignore.denies);
        expect(denies.length).toBe(2);

        const accepts = files.filter(gitignore.accepts);
        expect(accepts.length).toBe(2);
        expect(accepts[0]).toBe('/dir');
        expect(accepts[1]).toBe('/dir/');
    });

    // https://stackoverflow.com/a/5534865/1872200
    // https://stackoverflow.com/a/35279076/1872200
    it('should include file under ignored directory with /*', () => {
        const gitIgnoreContent = [
            '/application/*', // not ignore a folder itself
            '!/application/languages/',
        ].join('\n');
        /*
            The trailing /* is significant:
            The pattern dir/ excludes a directory named dir and (implicitly) everything under it.
            With dir/, Git will never look at anything under dir, 
            and thus will never apply any of the "un-exclude" patterns to anything under dir.

            The pattern dir/* says nothing about dir itself; it just excludes everything under dir. 
            With dir/*, Git will process the direct contents of dir, 
            giving other patterns a chance to "un-exclude" some bit of the content (!dir/sub/).
        */

        const gitignore = parser.compile(gitIgnoreContent);
        const files = [
            '/application/',
            '/application/languages/',
            '/application/languages/en.json',
        ];
        const denies = files.filter(gitignore.denies);
        expect(denies.length).toBe(0);

        const accepts = files.filter(gitignore.accepts);
        expect(accepts.length).toBe(3);
    });
});

