type Student = {Id:int; Name:string; Age:int}
let students = [
    {Id = 1; Name = "John"; Age = 13};
    {Id = 2; Name = "Moin"; Age = 21};
    {Id = 3; Name = "Bill"; Age = 18};
    {Id = 4; Name = "Ram"; Age = 20};
    {Id = 5; Name = "Ron"; Age = 13};
]

type SearchOption = SearchByName = 0 | SearchByAge = 1
type SearchCriteria =  {Option: SearchOption; Value: string}

let searchConditions = [
    {Option = SearchOption.SearchByName; Value = "John"}; 
    {Option = SearchOption.SearchByAge; Value = "13"}; 
]

let filteredStudents = 
    List.fold 
        (fun acc condition ->  
            let result = 
                match condition.Option with
                | SearchOption.SearchByName -> Seq.filter(fun s -> s.Name = condition.Value ) acc
                | SearchOption.SearchByAge -> Seq.filter(fun s -> s.Age = (int condition.Value)) acc
                | _ -> invalidOp (sprintf "Invalid option %s" (condition.Option.ToString()))

            result
        ) 
        (students |> List.toSeq)
        searchConditions 

for x in filteredStudents do printf "%s " x.Name
