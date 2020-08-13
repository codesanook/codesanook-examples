import * as parser from 'gitignore-parser';
// import * as fs from 'fs';

describe('git ignore /* pattern', () => {
/*
    Credit https://stackoverflow.com/a/38559600/1872200
    /dir will match a file, directory, link, anything named dir
    /dir/ will match only a directory named dir
    /dir/* will match all files, directories and anything else inside a directory named dir (but not the dir directory itself).
*/
    it('should ignore file and direcotry', () => {

        // const content = fs.readFileSync('.gitignore', 'utf8');
        const content = [
            // Match a file, directory, link or anything named dir
            '/dir',
        ].join('\n');

        const gitignore = parser.compile(content);
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
        const content = [
            // Match only a directory named dir
            '/dir/',
        ].join('\n');

        const gitignore = parser.compile(content);
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
        const content = [
            // Match all files, directories and anything else inside a directory named dir (but not the dir directory itself).
            '/dir/*',
        ].join('\n');

        const gitignore = parser.compile(content);
         
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
});
