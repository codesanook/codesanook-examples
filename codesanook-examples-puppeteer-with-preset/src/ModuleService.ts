import Dependency from "./Dependency";

export default class ModuleService {
    private readonly dependency: Dependency;
    constructor() {
        this.dependency = new Dependency();
    }

    init() {
        this.dependency.doSomething();
    }
}
