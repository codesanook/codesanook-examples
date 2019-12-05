import Dependency from '../src/Dependency'
import ModuleService from "../src/ModuleService";

// https://stackoverflow.com/a/56690970/1872200
// https://jestjs.io/docs/en/es6-class-mocks
const mockDoSomething = jest.fn();
jest.mock('../src/Dependency', () => {
    return {
        default: jest.fn().mockImplementation(() => {
            return { doSomething: mockDoSomething };
        })
    }
});

describe('', () => {
    it('test', () => {
        // const mockFn = jest.fn().mockImplementation((a) => {
        //     return {
        //         val: jest.fn()
        //     };
        // });
        // const element = new mockFn();

        const mockFn = jest.fn<JQuery<HTMLElement>, any[]>(() => {
            const element: any = {
                val: jest.fn()
            };
            return element as JQuery<HTMLElement>
        })

        const element = new mockFn();
        const moduleService = new ModuleService(element);
        moduleService.init();
        expect(Dependency).toHaveBeenCalledTimes(1);
        expect(mockDoSomething).toHaveBeenCalledTimes(1);
        expect(element.val).toHaveBeenCalledTimes(1);
    })
});
