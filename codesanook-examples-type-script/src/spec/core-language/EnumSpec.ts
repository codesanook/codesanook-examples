enum DatePartType {
    Date,
    Month,
    Year,
    Hour,
    Minute,
    Second,
}

describe('Enum', () => {
    it('should return correct Enumeration string value', () => {
        const value = DatePartType[DatePartType.Date];
        expect(value).toBe('Date');
    });
});