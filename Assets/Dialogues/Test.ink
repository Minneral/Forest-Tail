INCLUDE globals.ink

-> main

=== main ===

{talked:
    -> repeater
  - else:
    -> story
}


-> END


=== story ===
Hello, my name is Chloe # speaker:Chloe #portrait:chloe_default
Hi Chloe, I'm Yaroslav. What is your favorite color? # speaker:Yaroslav #portrait:player_default
    * [Red]
        -> color("Red")
    * [Green]
        -> color("Green")
    * [Blue]
        -> color("Blue")
-> END

=== repeater ===
I've talked to you already! # speaker:Chloe #portrait:chloe_default
-> END



=== color(val) ===
{val} is a very nice color! # speaker:Yaroslav #portrait:player_default
-> leave("Chloe")

=== leave(name) ===
Sorry, I need to leave you. # speaker:{name} #portrait:{name}_default
Bye then! # speaker: Yaroslav #portrait:player_default
~ talked = true
-> END