
|Category       |Recipe                         |XPath (1.0)                  |CSS                          | 
|---------------|-------------------------------|-----------------------------|-----------------------------|
|General        |Whole web page|xpath=/html|html|
|               |Whole web page body|xpath=/html/body|body|
|               |All text nodes of web page|//text()|NA|
|               |Element <E> by absolute reference|/html/body/.../.../.../E|body>…>…>…>E|
|Tag            |Element <E> anywhere on a page|//E|E|
|               |First <E> element anywhere on a page|//E[1] (index start from 1)|NA|
|               |Image element|//img|img|
|               |Element <E> that has attribute A|//E[@A]|E[A]|
|               |Element <E> that has attribute A containing text 't' exactly|//E[@A='t']|[A='t']|
|               |Element <E> that has attribute A containing text 't'|"//E[contains(@A|'t')]"|E[A*='t'] (== /value/)|
|               |Element <E> that has attribute A begins with 't'|"//E[starts-with(@A| 't')]"|[A^='t']|
|               |Element <E> that has attribute A ends with 't'|"//E[ substring( @A| string-length(@A) - string-length('t') +1 ) = 't' ]"|E[A$='t']|
|               |Element <E> with attribute A containing word 'w'|"//E[contains(concat(' '| @A| ' ')| ' w ')]"|css=E[A~='w']  (== /\bvalue\b/ in regex)|
|               |Element <E1> with id x or element <E2> with id y|//E1[@id='x']  |  //E2[@id='y']|"css=E1#I1|E2#I2"|
|               |Element <E1> with id x or id y|//E1[@id='x' or @id='y']|"css=E1#I1|E1#I2"|
|Attribute      |Attribute A of element <E>|//E/@A|NA|
|               |Attribute A of any elements|"//*/@A| //@A"|NA|
|               |Attribute A1 of element <E> where its attribute A2 is 't' exactly|//E[@A2='t']@A1|NA|
|               |Attribute A of element <E> where attribute A contains 't'|"//E[contains(@A|'t')]/@A"|NA|
|Id & Name      |Element <E> with id x|//E[@id='x']|E#x|
|               |Element with id x|"//*[@id='x']| //[@id='x']"|#x|
|               |Element <E> with name x|//E[@name='x']|E[name='x']|
|               |Element with name N|//*[@name='x']|[name='x']|
|               |Element with id x or a name y|//*[@id='x' or @name='y']|NA|
|               |Element with name x & at index 1 (1 based index )|//*[@name='x'][1]|[name=x]:nth-child(1)|
|               |Element with name x & specified value ‘y’|//*[@name='x'][@value='y']|[name=N][value='v’]|
|Lang & Class   |Element <E> is explicitly in language x|"//E[@lang='x' or starts-with(@lang| concat('x'| '-'))]"|E[lang|=x]|https://css-tricks.com/attribute-selectors/#rel-dash
|               |Element <E> is in language x|NA|E:lang(x)|
|               |Element with a class C|"//*[contains(concat(' '| @class| ' ')| ' C ')]"|.class-name|
|               |Element <E> with a class C|"//E[contains(concat(' '| @class| ' ')| ' C ')]"|E.class-name|
|Text & Link    |Any elements containing text 't' exactly|"//*[.='t']| //*[text()='t']"|NA|
|               |Element <E> containing text 't'|"//E[contains(text()|'t')]"|css=E:contains('t') |
|               |Link element|//a|css=a|
|               |<a> containing text 't' exactly|"//a[.='t']| //a[text()='t']"|NA|
|               |<a> containing text 't'|"//a[contains(text()|'t')]"|css=a:contains('t') |
|               |<a> with target link 'url'|//a[@href='url']|css=a[href='url']|
|               |Link URL labeled with text 't' exactly|//a[.='t']/@href|NA|
|Parent & Child |First child of element <E>|//E/*[1]|E > *:first-child|
|               |First <E> child|//E[1]|E:first-of-type|
|               |Last child of element E|//E/*[last()]|E *:last-child|
|               |Last <E> child|//E[last()]|css=E:last-of-type|
|               |Second <E> child|//E[2] OR //E/following-sibling::E|css=E:nth-of-type(2)|
|               |Second child that is an <E> element|//*[2][name()='E']|css=E:nth-child(2)|
|               |Second-to-last <E> child|//E[last()-1]|css=E:nth-last-of-type(2)|
|               |Second-to-last child that is an <E> element|//*[last()-1][name()='E']|css=E:nth-last-child(2)|
|               |Element <E1> with only <E2> children|//E1/[E2 and not( *[not(self::E2)])]|NA|
|               |Parent of element <E>|//E/..|NA|
|               |Descendant <E> of element with id I using specific path|//*[@id='I']/ . . ./. . ./. . ./E|css=#I > . . . > . . . > . . . > E|
|               |Descendant <E> of element with id I using unspecified path|//*[@id='I']//E|css=#I E|
|               |Element <E> with no children|//E[count(*)=0]|css=E:empty|
|               |Element <E> with an only child|//E[count(*)=1]|NA|
|               |Element <E> that is an only child|//E[count(preceding-sibling::*)+count(following-sibling::*)=0]|css=E:only-child|
|               |Element <E> with no <E> siblings|//E[count(../E) = 1]|css=E:only-of-type ⌦|
|               |Every Nth element starting with the (M+1)th|//E[position() mod N = M + 1]|css=E:nth-child(Nn + M)|
|Sibling        |Element <E1> following some sibling of <E2>|//E2/following-sibling::E1|css=E2 ~ E1|
|               |Element <E1> immediately following sibling <E2>|//E2/following-sibling::*[1][name()='E1']|css=E2 + E1|
|               |Element <E1> following sibling <E2> with one intermediary|//E2/following-sibling::*[2][name()='E1']|css=E2 + * + E1|
|               |Sibling element immediately following <E>|//E/following-sibling::*|css=E + *|
|               |Element <E1> preceding some sibling <E2>|//E2/preceding-sibling::E1|NA|
|               |Element <E1> immediately preceding sibling <E2>|//E2/preceding-sibling::*[1][name()='E1']|NA|
|               |Element <E1> preceding sibling <E2> with one intermediary|//E2/preceding-sibling::*[2][name()='E1']|NA|
|               |Sibling element immediately preceding <E>|//E/preceding-sibling::*[1]|NA|
|Table Cell     |"Cell by row and column (e.g. 3rd row| 2nd column)"|//*[@id='TestTable']//tr[3]//td[2]|css=#TestTable tr:nth-child(3) td:nth-child(2)|
|               |Cell immediately following cell containing 't' exactly|//td[preceding-sibling::td='t']|NA|
|               |Cell immediately following cell containing 't'|"//td[preceding-sibling::td[contains(.|'t')]]"|css=td:contains('t') ~ td|
|Dynamic        |User interface element <E> that is disabled|//E[@disabled]|css=E:disabled|
|               |User interface element that is enabled|//*[not(@disabled)]|css=*:enabled|
|               |Checkbox (or radio button) that is checked|//*[@checked]|css=*:checked|
|               |Element being designated by a pointing device|NA|css=E:hover|
|               |Element has keyboard input focus|NA|css=E:focus|
|               |Unvisited link|NA|css=E:link|
|               |Visited link|NA|css=E:visited|
|               |Active element|NA|E:active|


```
              ┌─────────────────┐
              │    ancestor     │
              └─────────────────┘
                     ┌───┐
                     │ 2 │
                     └─┬─┘
                       │
     ┌───────────┬─────┴─────┬───────────┐
   ┌─┴─┐       ┌─┴─┐       ┌─┴─┐       ┌─┴─┐
   │   │       │ 1 │       │   │       │   │
   └─┬─┘       └─┬─┘       └─┬─┘       └───┘
  ┌──┴──┐     ┌──┴──┐     ┌──┴──┐
┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┏━┻━┓ ┌─┴─┐ ┌─┴─┐
│   │ │   │ │   │ ┃   ┃ │   │ │   │
└───┘ └───┘ └───┘ ┗━┳━┛ └───┘ └───┘
              ┌─────┼─────┐
            ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
            │   │ │   │ │   │
            └───┘ └───┘ └───┘
```

```
              ┌─────────────────┐
              │ancestor-or-self │
              └─────────────────┘
                     ┌───┐
                     │ 3 │
                     └─┬─┘
                       │
     ┌───────────┬─────┴─────┬───────────┐
   ┌─┴─┐       ┌─┴─┐       ┌─┴─┐       ┌─┴─┐
   │   │       │ 2 │       │   │       │   │
   └─┬─┘       └─┬─┘       └─┬─┘       └───┘
  ┌──┴──┐     ┌──┴──┐     ┌──┴──┐
┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┏━┻━┓ ┌─┴─┐ ┌─┴─┐
│   │ │   │ │   │ ┃ 1 ┃ │   │ │   │
└───┘ └───┘ └───┘ ┗━┳━┛ └───┘ └───┘
              ┌─────┼─────┐
            ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
            │   │ │   │ │   │
            └───┘ └───┘ └───┘

```

```
              ┌─────────────────┐
              │      child      │
              └─────────────────┘
                     ┌───┐
                     │   │
                     └─┬─┘
                       │
     ┌───────────┬─────┴─────┬───────────┐
   ┌─┴─┐       ┏━┻━┓       ┌─┴─┐       ┌─┴─┐
   │   │       ┃   ┃       │   │       │   │
   └─┬─┘       ┗━┳━┛       └─┬─┘       └───┘
  ┌──┴──┐     ┌──┴──┐     ┌──┴──┐
┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
│   │ │   │ │ 1 │ │ 2 │ │   │ │   │
└───┘ └───┘ └───┘ └─┬─┘ └───┘ └───┘
              ┌─────┼─────┐
            ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
            │   │ │   │ │   │
            └───┘ └───┘ └───┘
```

```
              ┌─────────────────┐
              │   descendant    │
              └─────────────────┘
                     ┌───┐
                     │   │
                     └─┬─┘
                       │
     ┌───────────┬─────┴─────┬───────────┐
   ┌─┴─┐       ┏━┻━┓       ┌─┴─┐       ┌─┴─┐
   │   │       ┃   ┃       │   │       │   │
   └─┬─┘       ┗━┳━┛       └─┬─┘       └───┘
  ┌──┴──┐     ┌──┴──┐     ┌──┴──┐
┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
│   │ │   │ │ 1 │ │ 3 │ │   │ │   │
└───┘ └───┘ └─┬─┘ └─┬─┘ └───┘ └───┘
              │     ├─────┬─────┐
            ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
            │ 2 │ │ 4 │ │ 5 │ │ 6 │
            └───┘ └───┘ └───┘ └───┘
```

```
            ┌────────────────────┐
            │ descendant-or-self │
            └────────────────────┘
                     ┌───┐
                     │   │
                     └─┬─┘
                       │
     ┌───────────┬─────┴─────┬───────────┐
   ┌─┴─┐       ┏━┻━┓       ┌─┴─┐       ┌─┴─┐
   │   │       ┃ 1 ┃       │   │       │   │
   └─┬─┘       ┗━┳━┛       └─┬─┘       └───┘
  ┌──┴──┐     ┌──┴──┐     ┌──┴──┐
┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
│   │ │   │ │ 2 │ │ 4 │ │   │ │   │
└───┘ └───┘ └─┬─┘ └─┬─┘ └───┘ └───┘
              │     ├─────┬─────┐
            ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
            │ 3 │ │ 5 │ │ 6 │ │ 7 │
            └───┘ └───┘ └───┘ └───┘
```

```
             ┌───────────────────┐
             │     following     │
             └───────────────────┘
                     ┌───┐
                     │   │
                     └─┬─┘
                       │
     ┌───────────┬─────┴─────┬───────────┐
   ┌─┴─┐       ┏━┻━┓       ┌─┴─┐       ┌─┴─┐
   │   │       ┃   ┃       │ 1 │       │ 4 │
   └─┬─┘       ┗━┳━┛       └─┬─┘       └───┘
  ┌──┴──┐     ┌──┴──┐     ┌──┴──┐
┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
│   │ │   │ │   │ │   │ │ 2 │ │ 3 │
└───┘ └───┘ └─┬─┘ └─┬─┘ └───┘ └───┘
              │     ├─────┬─────┐
            ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
            │   │ │   │ │   │ │   │
            └───┘ └───┘ └───┘ └───┘
```

```
             ┌───────────────────┐
             │ following-sibling │
             └───────────────────┘
                     ┌───┐
                     │   │
                     └─┬─┘
                       │
     ┌───────────┬─────┴─────┬───────────┐
   ┌─┴─┐       ┏━┻━┓       ┌─┴─┐       ┌─┴─┐
   │   │       ┃   ┃       │ 1 │       │ 2 │
   └─┬─┘       ┗━┳━┛       └─┬─┘       └───┘
  ┌──┴──┐     ┌──┴──┐     ┌──┴──┐
┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
│   │ │   │ │   │ │   │ │   │ │   │
└───┘ └───┘ └─┬─┘ └─┬─┘ └───┘ └───┘
              │     ├─────┬─────┐
            ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
            │   │ │   │ │   │ │   │
            └───┘ └───┘ └───┘ └───┘
```

```
             ┌───────────────────┐
             │      parent       │
             └───────────────────┘
                     ┌───┐
                     │ 1 │
                     └─┬─┘
                       │
     ┌───────────┬─────┴─────┬───────────┐
   ┌─┴─┐       ┏━┻━┓       ┌─┴─┐       ┌─┴─┐
   │   │       ┃   ┃       │   │       │   │
   └─┬─┘       ┗━┳━┛       └─┬─┘       └───┘
  ┌──┴──┐     ┌──┴──┐     ┌──┴──┐
┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
│   │ │   │ │   │ │   │ │   │ │   │
└───┘ └───┘ └─┬─┘ └─┬─┘ └───┘ └───┘
              │     ├─────┬─────┐
            ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
            │   │ │   │ │   │ │   │
            └───┘ └───┘ └───┘ └───┘
```

```
             ┌───────────────────┐
             │     preceding     │
             └───────────────────┘
                     ┌───┐
                     │   │
                     └─┬─┘
                       │
     ┌───────────┬─────┴─────┬───────────┐
   ┌─┴─┐       ┏━┻━┓       ┌─┴─┐       ┌─┴─┐
   │ 3 │       ┃   ┃       │   │       │   │
   └─┬─┘       ┗━┳━┛       └─┬─┘       └───┘
  ┌──┴──┐     ┌──┴──┐     ┌──┴──┐
┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
│ 2 │ │ 1 │ │   │ │   │ │   │ │   │
└───┘ └───┘ └─┬─┘ └─┬─┘ └───┘ └───┘
              │     ├─────┬─────┐
            ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
            │   │ │   │ │   │ │   │
            └───┘ └───┘ └───┘ └───┘
```
```
             ┌───────────────────┐
             │ preceding-sibling │
             └───────────────────┘
                     ┌───┐
                     │   │
                     └─┬─┘
                       │
     ┌───────────┬─────┴─────┬───────────┐
   ┌─┴─┐       ┏━┻━┓       ┌─┴─┐       ┌─┴─┐
   │ 1 │       ┃   ┃       │   │       │   │
   └─┬─┘       ┗━┳━┛       └─┬─┘       └───┘
  ┌──┴──┐     ┌──┴──┐     ┌──┴──┐
┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
│   │ │   │ │   │ │   │ │   │ │   │
└───┘ └───┘ └─┬─┘ └─┬─┘ └───┘ └───┘
              │     ├─────┬─────┐
            ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
            │   │ │   │ │   │ │   │
            └───┘ └───┘ └───┘ └───┘
```  

```
             ┌───────────────────┐
             │       self        │
             └───────────────────┘
                     ┌───┐
                     │   │
                     └─┬─┘
                       │
     ┌───────────┬─────┴─────┬───────────┐
   ┌─┴─┐       ┏━┻━┓       ┌─┴─┐       ┌─┴─┐
   │   │       ┃ 1 ┃       │   │       │   │
   └─┬─┘       ┗━┳━┛       └─┬─┘       └───┘
  ┌──┴──┐     ┌──┴──┐     ┌──┴──┐
┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
│   │ │   │ │   │ │   │ │   │ │   │
└───┘ └───┘ └─┬─┘ └─┬─┘ └───┘ └───┘
              │     ├─────┬─────┐
            ┌─┴─┐ ┌─┴─┐ ┌─┴─┐ ┌─┴─┐
            │   │ │   │ │   │ │   │
            └───┘ └───┘ └───┘ └───┘
```

### Credit 
- XPath cheatsheet
https://www.red-gate.com/simple-talk/dotnet/.net-framework/xpath,-css,-dom-and-selenium-the-rosetta-stone/
- XPath Axes diagram XSLT 2.0 and XPath 2.0 Programmer's Reference, 4th Edition
