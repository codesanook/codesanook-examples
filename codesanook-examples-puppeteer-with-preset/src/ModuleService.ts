import Dependency from "./Dependency";

export default class ModuleService {
    private readonly dependency: Dependency;
    constructor(private element: JQuery<HTMLElement>) {
        this.dependency = new Dependency();
    }

    init() {
        this.dependency.doSomething();
        console.log(`element value: ${this.element.val()}`);
    }
}
