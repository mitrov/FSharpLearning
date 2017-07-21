module RecursiveTypes

type Book = {title: string; price: decimal}

type ChocolateType = Dark | Milk | SeventyPercent
type Chocolate = {chocType: ChocolateType ; price: decimal}

type WrappingPaperStyle = 
    | HappyBirthday
    | HappyHolidays
    | SolidColor

type Gift =
    | Book of Book
    | Chocolate of Chocolate 
    | Wrapped of Gift * WrappingPaperStyle
    | Boxed of Gift 
    | WithACard of Gift * message:string

// a Book
let wolfHall = {title="Wolf Hall"; price=20m}

// a Chocolate
let yummyChoc = {chocType=SeventyPercent; price=5m}

// A Gift
let birthdayPresent = WithACard (Wrapped (Book wolfHall, HappyBirthday), "Happy Birthday")
// WithACard (
// Wrapped (
// Book {title = "Wolf Hall"; price = 20M},
// HappyBirthday),
// "Happy Birthday")

// A Gift
let christmasPresent = Wrapped (Boxed (Chocolate yummyChoc), HappyHolidays)
// Wrapped (
// Boxed (
// Chocolate {chocType = SeventyPercent; price = 5M}),
// HappyHolidays)

let rec description gift =
    match gift with 
    | Book book -> 
        sprintf "'%s'" book.title 
    | Chocolate choc -> 
        sprintf "%A chocolate" choc.chocType
    | Wrapped (innerGift,style) -> 
        sprintf "%s wrapped in %A paper" (description innerGift) style
    | Boxed innerGift -> 
        sprintf "%s in a box" (description innerGift) 
    | WithACard (innerGift,message) -> 
        sprintf "%s with a card saying '%s'" (description innerGift) message

birthdayPresent |> description  

christmasPresent |> description

let rec totalCost gift =
    match gift with 
    | Book book -> 
        book.price
    | Chocolate choc-> 
        choc.price
    | Wrapped (innerGift,style) -> 
        (totalCost innerGift) + 0.5m
    | Boxed innerGift -> 
        (totalCost innerGift) + 1.0m
    | WithACard (innerGift,message) -> 
        (totalCost innerGift) + 2.0m  

birthdayPresent |> totalCost 
// 22.5m

christmasPresent |> totalCost 
// 6.5m

let rec whatsInside gift =
    match gift with 
    | Book book -> 
        "A book"
    | Chocolate choc -> 
        "Some chocolate"
    | Wrapped (innerGift,style) -> 
        whatsInside innerGift
    | Boxed innerGift -> 
        whatsInside innerGift
    | WithACard (innerGift,message) -> 
        whatsInside innerGift

birthdayPresent |> whatsInside 
// "A book"

christmasPresent |> whatsInside 
// "Some chocolate"

let rec cataGift fBook fChocolate fWrapped fBox fCard gift :'r =
    let recurse = cataGift fBook fChocolate fWrapped fBox fCard
    match gift with 
    | Book book -> 
        fBook book
    | Chocolate choc -> 
        fChocolate choc
    | Wrapped (gift,style) -> 
        fWrapped (recurse gift,style)
    | Boxed gift -> 
        fBox (recurse gift)
    | WithACard (gift,message) -> 
        fCard (recurse gift,message) 

let totalCostUsingCata gift =
    let fBook (book:Book) = 
        book.price
    let fChocolate (choc:Chocolate) = 
        choc.price
    let fWrapped  (innerCost,style) = 
        innerCost + 0.5m
    let fBox innerCost = 
        innerCost + 1.0m
    let fCard (innerCost,message) = 
        innerCost + 2.0m
    // call the catamorphism
    cataGift fBook fChocolate fWrapped fBox fCard gift

birthdayPresent |> totalCostUsingCata 


let descriptionUsingCata gift =
    let fBook (book:Book) = 
        sprintf "'%s'" book.title 
    let fChocolate (choc:Chocolate) = 
        sprintf "%A chocolate" choc.chocType
    let fWrapped (innerText,style) = 
        sprintf "%s wrapped in %A paper" innerText style
    let fBox innerText = 
        sprintf "%s in a box" innerText
    let fCard (innerText,message) = 
        sprintf "%s with a card saying '%s'" innerText message
    // call the catamorphism
    cataGift fBook fChocolate fWrapped fBox fCard gift

birthdayPresent |> descriptionUsingCata  
// "'Wolf Hall' wrapped in HappyBirthday paper with a card saying 'Happy Birthday'"

christmasPresent |> descriptionUsingCata  
// "SeventyPercent chocolate in a box wrapped in HappyHolidays paper"
