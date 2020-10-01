import moment from 'moment'

describe('moment UTC', function () {
    it('change to correct date', () => {
        // let resultDate = moment().utcOffset('+07:00');
        let input = '2018-04-06T03:00:00Z'.toUpperCase();

        // Create UTC object
        // let utc = moment.utc(input).utcOffset();

        let localDate = moment.utc(input).utcOffset(7);

        //mutate
        expect(localDate.format('YYYY-MM-DD HH:mm:ss Z')).toBe('2018-04-06 10:00:00 +07:00');
        expect(localDate.utcOffset()).toBe(7 * 60);

        expect(localDate.year()).toBe(2018);
        expect(localDate.month()).toBe(3);//value is start from 0
        expect(localDate.date()).toBe(6);

        expect(localDate.hour()).toBe(10);//local time
        expect(localDate.minute()).toBe(0);
        expect(localDate.second()).toBe(0);
    });

    it('set to correct date', () => {

        //var resultDate = moment().utcOffset('+07:00');
        //2018-04-06T03:00:45.497Z
        //2018-04-06T03:00:00.000Z

        var input = '2018-04-06T10:00:00+07:00';
        //create UTC object
        var localDate = moment(input, 'YYYY-MM-DDTHH:mm:ssZZ');
        //mutate
        expect(localDate.utcOffset()).toBe(7 * 60);
        expect(localDate.toISOString()).toBe('2018-04-06T03:00:00.000Z');
    });

    it('to ISO string', () => {

        //2018-04-06T10:53:52Z
        var input = '2018-01-01T00:00:00.000+07:00';
        var resultDate = moment(input);

        expect(resultDate.toISOString()).toBe('2017-12-31T17:00:00.000Z');
    });
});