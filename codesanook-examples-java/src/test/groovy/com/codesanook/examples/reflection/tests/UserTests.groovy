package com.codesanook.examples.reflection.tests

import com.codesanook.examples.reflection.User
import spock.lang.Specification

class UserTests extends Specification {
    def "getFullName should return correct full name"() {
        given:
            def user =new User()
            user.setFirstName("AA")
            user.setLastName("BB")
        when:
            def result = user.getFullName()
        then:
            result == "AA BB"
    }
}
