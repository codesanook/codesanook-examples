//ES6 export default to a User class 
export default class User {

    private _firstName: string;
    private _lastName: string;

    // property and method are public by default
    get firstName(): string {
        return this._firstName;
    }

    set firstName(value: string) {
        this._firstName = value;
    }

    get lastName(): string {
        return this._lastName;
    }

    set lastName(value: string) {
        this._lastName = value;
    }

    getFullName(): string {
        return `${this._firstName} ${this._lastName}`
    }

    saySomething(message: string, time: number): string[] {
        var totalMessages: string[] = [];
        for (let index = 0; index < time; index++) {
            totalMessages.push(message);
        }
        return totalMessages;
    }
}
