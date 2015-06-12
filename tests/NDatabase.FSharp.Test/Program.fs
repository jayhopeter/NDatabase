// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

type Person = { FirstName:string; LastName:string; Age:int } 
  with override x.ToString() = 
    sprintf "%s %s is %d years old" x.FirstName x.LastName x.Age 
       
[<EntryPoint>]
let main argv = 
    let p1 = { FirstName = "Jacek"; LastName = "Spolnik"; Age = 25 } 

    let odb = NDatabase.OdbFactory.Open("fsharp.ndb")

    let oid = odb.Store(p1)

    odb.Dispose()

    let odb = NDatabase.OdbFactory.Open("fsharp.ndb")

    let result = odb.QueryAndExecute<Person>().GetFirst()

    odb.Dispose();

    printfn "%s" (result.ToString())
    
    0 // return an integer exit code
