---
Title: Functional Card Deck in C#
Subtitle: 
Date: 2018-10-11
Tags: c#, fp
---

I've been inspired lately by Mark Seemann's series of posts about [Applicative Functors](http://blog.ploeh.dk/2018/10/01/applicative-functors/). One of the latest posts is an example about
creating a [full deck](http://blog.ploeh.dk/2018/10/08/full-deck/) of cards. Most of posts up to this
point have contained a C# example but for some reason this one didn't. This inspired me to take a shot at it.

<!--more-->

I also decided to implement shuffling and dealing of hands:

```c#
using System;
using System.Linq;

namespace FullDeck
{
    class Program
    {
        enum Suit { Diamonds, Hearts, Clubs, Spades }

        enum Face { Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }

        struct Card { public Suit Suit; public Face Face; }

        static void Main(string[] args)
        {
            var allFaces = (Face[])Enum.GetValues(typeof(Face));

            var allSuits = (Suit[])Enum.GetValues(typeof(Suit));

            var fullDeck = allSuits.SelectMany(x => allFaces.Select(y => new Card { Suit = x, Face = y }));

            Console.WriteLine("=== FullDeck ===");
            foreach (var card in fullDeck)
                Console.WriteLine($"{card.Suit} {card.Face}");
            Console.WriteLine();

            var shuffledDeck = fullDeck.OrderBy(x => Guid.NewGuid());
            
            Console.WriteLine("=== ShuffledDeck ===");
            foreach (var card in shuffledDeck)
                Console.WriteLine($"{card.Suit} {card.Face}");
            Console.WriteLine();

            // Everytime shuffledDeck is iterated over the order changes.

            var numberOfHands = 4;
            var numberOfCardsPerHand = 5;
            var hands = shuffledDeck.Take(numberOfHands * numberOfCardsPerHand)
                .Select((x, i) => new { x, i })
                .GroupBy(x => x.i % numberOfHands)
                .Select(g => g.Select(x => x.x));
            
            Console.WriteLine("=== Hands ===");
            foreach (var hand in hands)
            {
                foreach (var card in hand)
                {
                    Console.WriteLine($"{card.Suit} {card.Face}");
                }
                Console.WriteLine();                
            }
            Console.WriteLine();
        }
    }
}
```
