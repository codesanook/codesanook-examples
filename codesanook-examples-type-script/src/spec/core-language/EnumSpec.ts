
export enum DatePartType {
    date,
    month,
    year,
    hour,
    minute,
    second,
}


describe("enum", () => {
    it("should return correct string value", () => {

        var value = DatePartType.date

        console.log(DatePartType[value]);

    });
});