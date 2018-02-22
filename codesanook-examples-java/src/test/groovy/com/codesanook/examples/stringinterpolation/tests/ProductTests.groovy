package com.codesanook.examples.stringinterpolation.tests

import spock.lang.Specification
import com.codesanook.examples.stringinterpolation.Product
import java.text.SimpleDateFormat

class ProductTests extends Specification {

    def "string interpolation Java style should return correct formatted string"() {
        given:
            Product product = new Product();
            product.setName("Basic Programming");
            product.setPrice(499.99);

            Date date = new GregorianCalendar(2018, 1, 5).getTime(); //month value in Java starts from 0
            product.setCreateDate(date);

        when:
            SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyy-MMM-dd", Locale.ENGLISH);
            String formattedDate = simpleDateFormat.format(product.getCreateDate());
            String formattedString = String.format("product %s costs %.2f is added on %s",
                product.getName(),
                product.getPrice(),
                formattedDate);

        then:
            formattedString == "product Basic Programming costs 499.99 is added on 2018-Feb-05";
    }

    def "string interpolation Groovy style should return correct formatted string"() {
        given:
            def date = new GregorianCalendar(2018, 1, 5).getTime()
            //initialize property from constructor
            def product = new Product(name : "Basic Programming", price : 499.99, createDate : date)

        when:
            def simpleDateFormat = new SimpleDateFormat("yyyy-MMM-dd", Locale.ENGLISH)
            def formattedDate = simpleDateFormat.format(product.getCreateDate())
            def formattedString = "product ${product.name} costs ${String.format('%.2f',product.price)} is added on $formattedDate"

        then:
            formattedString == "product Basic Programming costs 499.99 is added on 2018-Feb-05"
    }
}



