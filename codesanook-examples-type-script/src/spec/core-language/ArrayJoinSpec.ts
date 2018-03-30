describe("array.join", () => {
        it("should return a correct string", () => {
            //given
            let fruits = ["apple", "papaya", "banana"];

            //when
            let joinedFruits = fruits.join(",");

            //then
            expect(joinedFruits).toBe("apple,papaya,banana");
        });
});