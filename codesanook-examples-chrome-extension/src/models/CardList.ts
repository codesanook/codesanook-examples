import * as $ from "jquery"

export class CardList {
    //match ending with (1),[1], [time box 1], [TimeBox 1], (time box 1) or (TimeBox 1) 
    private readonly pointPattern: RegExp = /[\(\[][^\d]*(\d+)[\]\)]\s*$/i;
    private static readonly sprintTitle = "sprint\\s*\\d*\\s*committed\\s*\\d{8}";
    private static readonly cardListNames = [
        "qa \\+ code reviews",
        "in progress",
        CardList.sprintTitle
    ].join("|");

    //properties
    private _title: string;
    get title(): string {
        return this._title;
    }
    set title(value: string) {
        this._title = value;
    }

    private _point: number;
    public get point(): number {
        return this._point;
    }
    public set point(value: number) {
        this._point = value;
    }

    constructor(private element: HTMLElement) {
        this.title = $(this.element).find(".list-header-name").val();
        let pointsInList = this.getPointsOfAllCardsInTheList();
        this.point = pointsInList.reduce((total, point) => total + point, 0);
    }

    setPoint(
        cssSelector: string,
        format: (list: CardList) => string): void {

        let header = $(this.element).find(".list-header");
        header.find(cssSelector).remove();
        header
            .find(".list-header-name")
            .after(`<span class='${cssSelector.substring(1)} point-block'>${format(this)}</span>`);
    }

    private getPointsOfAllCardsInTheList(): number[] {

        return $(this.element).find(".list-card")
            .toArray()
            .map((card): number => {
                let cardTitle = $(card).find(".list-card-title").text();
                let match = this.pointPattern.exec(cardTitle);
                return match ? parseInt(match[1]) : 0;
            });
    }

    static createCardLists(): CardList[] {
        let cardListsRegex = new RegExp(CardList.cardListNames, "i");
        let cardListTitles = $(".list")
            .filter((_, list) => {
                let listTitle = $(list).find(".list-header-name").val();
                return cardListsRegex.test(listTitle);
            })
            .toArray();

        //transform to strongly type
        return cardListTitles.map(list => new CardList(list));
    }

    //TODO extract the latest sprint committed to prevent order problem 
    static getSprintList(cardLists: CardList[]): CardList {
        let sprintTitlePattern = new RegExp(CardList.sprintTitle, "i");
        return cardLists.filter(list => sprintTitlePattern.test(list.title))[0];
    }

    static getTotalPoint(cardLists: CardList[]): number {
        return cardLists.reduce((total, list) => total + list.point, 0);
    }
}