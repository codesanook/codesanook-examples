export default class Product {

    private _name: string;
    private _createDate: Date;
    private _price: number;

    get name(): string {
        return this._name;
    }

    set name(value: string) {
        this._name = value;
    }

    get createDate(): Date {
        return this._createDate;
    }

    set createDate(value: Date) {
        this._createDate = value;
    }

    get price(): number {
        return this._price;
    }

    set price(value:number){
        this._price = value;    
    } 
}