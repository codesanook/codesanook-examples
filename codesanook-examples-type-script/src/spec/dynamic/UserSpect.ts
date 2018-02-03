import User from '../../dynamic/User';
//userful links
//https://jasmine.github.io/tutorials/your_first_suite

//create a nested describe block which make a group of specs and shared context  
describe('call user object', () => {
    describe('getFullName method ', function () {
        it('return correct full name', () => {
            //given
            let user = new User();
            user.firstName = 'Anthony';
            user.lastName = 'CodeSanook';

            //when
            let fullName = user.getFullName();

            //then
            expect(fullName).toBe('Anthony CodeSanook');
        });

        it('with dynamic string name should return correct full name', () => {

            //given
            let user = new User();
            user['firstName'] = 'Anthony';
            user['lastName'] = 'CodeSanook';

            //when
            let fullName = user['getFullName']();

            //then
            expect(fullName).toBe('Anthony CodeSanook');

        });

        it('with apply should return correct full name', () => {

            //given
            let userA = new User();

            let userB = new User();
            userB['firstName'] = 'Anthony';
            userB['lastName'] = 'CodeSanook';

            //when
            let fullName = userA.getFullName.apply(userB);
            //then
            expect(fullName).toBe('Anthony CodeSanook');
        });
    });//end describe

    describe('saySomething method ', () => {
        it('with apply function should return correct total message count', () => {
            //given
            let userA = new User();

            let userB = new User();
            userB['firstName'] = 'Anthony';
            userB['lastName'] = 'CodeSanook';

            //when
            //use apply we pass arguments as array 
            let messages = userA.saySomething.apply(userB, ['Hello World with apply', 5]);
            //then
            expect(messages.length).toBe(5);
            console.log(messages.join('\n'));
        });

        it('with call function should return correct total message count', () => {
            //given
            let userA = new User();
            let userB = new User();
            userB['firstName'] = 'Anthony';
            userB['lastName'] = 'CodeSanook';

            //when
            //use apply we pass arguments as comma separated values
            let messages = userA.saySomething.call(userB, 'Hello World with call', 10);

            //then
            expect(messages.length).toBe(10);
            console.log(messages.join('\n'));
        });
    });//end describe

    it('should return all members', () => {
        //given
        let user = new User();
        let userKeys: string[] = [];

        //when
        for (let key in user) {
            userKeys.push(key);
        }

        //then
        expect(userKeys).toEqual(jasmine.arrayContaining(
            [
                'firstName',
                'lastName',
                'getFullName'
            ]));
    });

    // like set up in junit
    beforeEach(() => {
    });

    //like tear down in junit
    afterEach(() => {
    });

    //like test fixture set up  in junit
    beforeAll(() => {
    });

    //like test fixture set up  in junit
    afterAll(function () {
    });
});//end of parent describe 