-> main

=== main ===
Hello, my name is Chloe # speaker: Chloe
Hi Chloe, I'm Yaroslav. What is your favorite color? # speaker: Yaroslav
    * [Red]
        -> color("Red")
    * [Green]
        -> color("Green")
    * [Blue]
        -> color("Blue")
-> END


=== color(val) ===
{val} is a very nice color! # speaker: Yaroslav
-> leave("Chloe")

=== leave(name) ===
Sorry, I need to leave you. # speaker: {name}
Bye then! # speaker: Yaroslav
-> END