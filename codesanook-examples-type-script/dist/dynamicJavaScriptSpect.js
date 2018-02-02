function add(x, y) {
    return x + y;
}
describe('calculator', function () {
    it('should add two numbers', function () {
        expect(add(1, 2)).toBe(4);
    });
});
