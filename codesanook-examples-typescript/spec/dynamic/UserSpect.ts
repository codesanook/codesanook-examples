import User from '../../src/models/User';
// Useful links:
// https://jasmine.github.io/tutorials/your_first_suite

// Create a nested describe block which make a group of specs and shared context  
describe('call user object\'s members', () => {
    describe('with normal style', () => {
        it('should return a correct full name', () => {
            //given
            const user = new User();
            user.firstName = 'Anthony';
            user.lastName = 'CodeSanook';

            //when
            const fullName = user.getFullName();

            //then
            expect(fullName).toBe('Anthony CodeSanook');
        });
    });

    describe('with dynamic style', () => {
        it('should return a correct full name', () => {
            //given
            const user = new User();
            user['firstName'] = 'Anthony';
            user['lastName'] = 'CodeSanook';

            //when
            const fullName = user['getFullName']();

            //then
            expect(fullName).toBe('Anthony CodeSanook');
        });
    });//end describe

    describe('with apply function ', () => {
        it('should return a correct full name', () => {
            //given
            const userA = new User();

            const userB = new User();
            userB['firstName'] = 'Anthony';
            userB['lastName'] = 'CodeSanook';

            //when
            //use apply we pass arguments as array 
            const fullName = userA.getFullName.apply(userB);

            //then
            expect(fullName).toBe('Anthony CodeSanook');
        });

        it('should return correct total message count', () => {
            //given
            const userA = new User();

            const userB = new User();
            userB['firstName'] = 'Anthony';
            userB['lastName'] = 'CodeSanook';

            //when
            //use apply we pass arguments as array 
            const messages = userA.saySomething.apply(userB, ['Hello World with apply', 5]);
            //then
            expect(messages.length).toBe(5);
        });
    });//end describe

    describe('with call function', () => {
        it('should return a correct full name', () => {
            //given
            const userA = new User();

            const userB = new User();
            userB['firstName'] = 'Anthony';
            userB['lastName'] = 'CodeSanook';

            //when
            //use apply we pass arguments as array 
            const fullName = userA.getFullName.call(userB);

            //then
            expect(fullName).toBe('Anthony CodeSanook');
        });

        it('should return correct total message count', () => {
            //given
            const userA = new User();
            const userB = new User();
            userB['firstName'] = 'Anthony';
            userB['lastName'] = 'CodeSanook';

            //when
            //use apply we pass arguments as comma separated values
            const messages = userA.saySomething.call(userB, 'Hello World with call', 10);

            //then
            expect(messages.length).toBe(10);
        });
    });//end describe

    it('should return only getter property name', () => {
        // Given
        const user = new User();
        const prototype = Object.getPrototypeOf(user);
        // Properties are define in a prototype https://stackoverflow.com/a/37643453/1872200
        const properties = Object.getOwnPropertyNames(prototype).filter(name => {
            const description = Object.getOwnPropertyDescriptor(prototype, name);
            return typeof description["get"] === "function";
        });

        // Then
        expect(properties).toEqual(jasmine.arrayContaining(
            [
                'firstName',
                'lastName',
            ]
        ));
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
    afterAll(() => {
    });
});//end of parent describe 