import moment from 'moment';
import Product from '../../src/models/Product';

describe('string interpolation return correct formatted string', function () {
    it('return correct full name', () => {
        //given
        let product = new Product();
        product.name = 'Basic Programming';
        product.price = 499.99;
        product.createDate = new Date(2018, 1, 5); //month value in JavaScript starts from 0
        //when
        //format date with moment js
        var formattedString =
            `product ${product.name} costs ${product.price} is added on ${moment(product.createDate).format("YYYY-MMM-DD")}`;

        //then
        expect(formattedString).toBe('product Basic Programming costs 499.99 is added on 2018-Feb-05');
    });
});