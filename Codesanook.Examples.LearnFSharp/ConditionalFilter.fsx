class Program {
0 references
static void Main (string[] args) { studentLinq ();
}
1 reference
static void studentLinq () j|
// Student collection
IList<Student> studentList = new	List<Student> ()	{		
new	Student	0 {	StudentID =	1, StudentName =	"John"	, Age	= 13 },
new	Student	0 {	StudentID =	2, StudentName =	"Moin"	, Age	= 21 },
new	Student	0 {	StudentID =	3, StudentName =	"Bill"	, Age	= 18 },
new	Student	0 {	StudentID =	4, StudentName =	"Ram",	Age =	20 },
new	Student	0 {	StudentID =	5, StudentName =	"Ron",	Age =	15 }
};
string conditionName = "SearchByName";
Func<Student, bool> myCriteria =null; if (conditionName == "SearchByAge")
{
myCriteria = s => s.Age > 12 && s.Age < 20;
}else if (conditionName == "SearchByName"){ myCriteria = s => s.StudentName == "Bill";
}else {
throw new NullReferenceException("Your not definied condition for search
}
List<Student> teenAgerStudent = studentList.Where(myCriteria).ToList<Student>()
foreach (Student item in teenAgerStudent) {
Console.WriteLine (item.StudentName);
}
}

