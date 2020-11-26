using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqToObjects
{
    public class Esercizio
    {
        //Creazioni liste
        public static List<Product> createProductList()
        {
            var lista = new List<Product>
            {
                new Product{ID=1,Name="Telefono",UnitPrice=300.99},
                new Product{ID=2,Name="Computer",UnitPrice=800},
                new Product{ID=3,Name="Tablet",UnitPrice=550.99},
            };
            return lista;
        }

        public static List<Order> createOrderList()
        {
            var lista = new List<Order>(); //dichiaro la lista vuota poi aggiungo gli elementi-> è un altro metodo rispetto a sopra

            var order = new Order { ID = 1, ProductID = 1, Quantity = 4 };
            lista.Add(order);

            var order1 = new Order { ID = 2, ProductID = 2, Quantity = 1 };
            lista.Add(order1);

            var order2 = new Order { ID = 3, ProductID = 1, Quantity = 1 };
            lista.Add(order2);

            return lista;
        }

        //Esecuzione immediata e ritardata
        public static void DeferredExecution()
        {
            var productList = createProductList();
            var orderList = createOrderList();

            //Visualizzazione dei risultati
            foreach (var p in productList)
            {
                Console.WriteLine("{0} - {1} - {2}",p.ID,p.Name, p.UnitPrice);
            }

            foreach (var p in orderList)
            {
                Console.WriteLine("{0} - {1} - {2}", p.ID, p.ProductID, p.Quantity);
            }

            //--------------------------------------------------------------------------------
            //---------------------------ESECUZIONE DIFFERITA---------------------------------------

            //Creazione query
            var list = productList.Where(product => product.UnitPrice >= 400) //prodotto tale che il prezzo del prodotto sia maggiore di 4000---product è un parametro di ingresso, posso mettere qualsiasi nome
                .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice });

            //Aggiunta prodotto 
            productList.Add(new Product { ID = 4, Name = "Bici", UnitPrice = 500.99 });

            //risultati
            Console.WriteLine("Esecuzione Differita: ");
            foreach (var p in list)
            {
                Console.WriteLine("{0} - {1}", p.Nome, p.Prezzo); //il campo aggiunto dopo la creazione della query lo visualizzo, perchè la creazione della query è una sorta di dichiarazione della stessa, non un'esecuzione.
            }

            //--------------------------------------------------------------------------
            //--------------------------ESECUZIONE IMMEDIATA-----------------------------------

            //creazione query ed esecuzione
            var list1 = productList.Where(p => p.UnitPrice >= 400)
                .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice })
                .ToList();

            //Aggiunta prodotto 
            productList.Add(new Product { ID = 5, Name = "Divano", UnitPrice = 450.99 });

            //Risultati
            Console.WriteLine("Esecuzione Immediata: ");
            foreach (var p in list1)
            {
                Console.WriteLine("{0} - {1}", p.Nome, p.Prezzo); //il campo aggiunto dopo la creazione della query non lo visualizzo, perchè la query è stata creata ed eseguita subito.
            }
           
        }

        //Sintassi
        public static void Syntax()
        {
            var productList = createProductList();
            var orderList = createOrderList();

            //Method syntax
            var methodList=productList
                .Where(p => p.UnitPrice <=600) //prodotto tale che il prezzo del prodotto sia maggiore di 4000---product è un parametro di ingresso, posso mettere qualsiasi nome
                .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice })
                .ToList(); //casting di un ienumerable in lista

            //Query
            var queryList =
                (from p in productList
                where p.UnitPrice <= 600
                select new { Nome = p.Name, Prezzo = p.UnitPrice }).ToList();
        }

        //Operatori
        public static void Operators()
        {
            var productList = createProductList();
            var orderList = createOrderList();

            //Visualizzazione dei risultati
            Console.WriteLine("Lista Prodotti: ");
            foreach (var p in productList)
            {
                Console.WriteLine("{0} - {1} - {2}", p.ID, p.Name, p.UnitPrice);
            }

            Console.WriteLine("Ordini: ");
            foreach (var p in orderList)
            {
                Console.WriteLine("{0} - {1} - {2}", p.ID, p.ProductID, p.Quantity);
            }

            //Filtro OfType
            var list = new ArrayList();
            list.Add(productList);
            list.Add("Ciao!");
            list.Add(123);

            var typeQuery =
                from item in list.OfType<int>() //posso mettere anche <List<Product>> e sistemare anhe il writeline
                select item;

            Console.WriteLine("OfType: ");
            foreach (var item in typeQuery)
            {
                Console.WriteLine(item);
            }

            //Element
            Console.WriteLine("Elementi: ");
            int[] empty = { };
            var el1 = empty.FirstOrDefault();
            Console.WriteLine(el1);

            var p1 = productList.ElementAt(0).Name; //mi stampa il nome del primo elemento
            Console.WriteLine(p1);

            //Ordinamento
            Console.WriteLine("Ordinamento: ");

            //productList.Add(new Product { ID = 4, Name = "Telefono", UnitPrice = 1000 });

            //1 modo->Querysintax
            //ordino i prodotti per nome e prezzo
            var orderedList =
                from p in productList
                orderby p.Name ascending, p.UnitPrice descending
                select new { Nome = p.Name, Prezzo = p.UnitPrice };

            //2 modo--rispetto a prima ho il reverse
            var orderedList2 = productList
                .OrderBy(p => p.Name)
                .ThenByDescending(p => p.UnitPrice)
                .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice })
                .Reverse();

            foreach (var item in orderedList)
            {
                Console.WriteLine("{0} - {1}", item.Nome, item.Prezzo);
            }

            Console.WriteLine("Secondo modo: ");
            foreach(var item in orderedList2)
            {
                Console.WriteLine("{0} - {1}", item.Nome, item.Prezzo);
            }


            //Quantificatori
            var hasProductWithT = productList.Any(p => p.Name.StartsWith("T")); //any mi dà come risultato un booleano->almeno un elemento della lista deve iniziare con t-> mi restituisce un true
            var allProductWithT = productList.All(p => p.Name.StartsWith("T")); //tutti i prodotti della lista devono iniziare con la T-> mi restituisce un false
            Console.WriteLine("Ci sono prodotti che iniziano con la t? {0}", hasProductWithT);
            Console.WriteLine("Tutti i prodotti iniziano con la t? {0}",allProductWithT);

            //GroupNy
            Console.WriteLine("GroupBy: ");

            //QuerySintax
            //raggruppiamo order per ProductID
            var groupBylist =    //dentro ho una chiave che rappresenta il raggruppamento che ho fatto e poi gli ordini associati a quella chiave(Chiave=> campo per cui ho raggruppato)
                from o in orderList
                group o by o.ProductID into groupList
                select groupList;
            Console.WriteLine("QuerySintax");
            foreach (var order in groupBylist)
            {
                Console.WriteLine(order.Key);
                foreach (var item in order)
                {
                    Console.WriteLine($"\t {item.ProductID} - {item.Quantity}");
                }
            }

            //MethodSyntax
            var groupbyList2 =
                orderList
                .GroupBy(o => o.ProductID);
            Console.WriteLine("MethodSintax");
            foreach (var order in groupbyList2)
            {
                Console.WriteLine(order.Key);
                foreach (var item in order)
                {
                    Console.WriteLine("{0} - {1}", item.ProductID, item.Quantity);
                }
            }

            //GroupBy con funzione di aggregazione(SUM)
            //Raggruppare gli ordini per prodotto e ricavare la somma delle quantità.
            var sumQuantityByProduct =
                orderList
                .GroupBy(p => p.ProductID)
                .Select(lista => new {
                    Id = lista.Key,
                    Quantities = lista.Sum(p => p.Quantity)
                });

            Console.WriteLine("GroupBy con aggregato: Method syntax");
            foreach (var item in sumQuantityByProduct)
            {
                Console.WriteLine("{0} - {1}",item.Id, item.Quantities);
            }

            //QuerySyntax
            var sumByProduct =
                from o in orderList
                group o by o.ProductID into list3
                select new { Id = list3.Key, Quantities = list3.Sum(x => x.Quantity) };

            Console.WriteLine("GroupBy con aggregato: Query syntax");
            foreach (var item in sumByProduct)
            {
                Console.WriteLine("{0} - {1}", item.Id, item.Quantities);
            }

            //JOIN-> sempre inner join
            //recuperiamo i prodotti che hanno ordini
            //Nome(da lista prodotti)-Id ordine-Quantità(da lista di ordini)
            Console.WriteLine("JOIN: Method Syntax");

            //Method Syntax
            var joinList = productList
                .Join(orderList,        //seconda lista su cui fare la join
                p => p.ID,                //prima chiave
                o => o.ProductID,         //seconda chiave
                (p, o) => new { Nome = p.Name, OrderId = o.ID, Quantità = o.Quantity });

            foreach (var item in joinList)
            {
                Console.WriteLine("{0} - {1} - {2}", item.Nome, item.OrderId, item.Quantità);
            }

            //Query Syntax
            Console.WriteLine("JOIN: Query Syntax");

            var joinList2 =
                from p in productList
                join o in orderList
                on p.ID equals o.ProductID       
                select new
                {
                    Nome = p.Name,
                    OrderId = o.ID,
                    Quantità = o.Quantity
                };

            foreach (var item in joinList2)
            {
                Console.WriteLine("{0} - {1} - {2}", item.Nome, item.OrderId, item.Quantità);
            }

            //GROUPJOIN
            //Recuperare gli ordini per prodotto e somma quantità
            //Nome prodotto -Quantità totale
            Console.WriteLine("GROUPJOIN: Method Syntax");
            var groupJoinList = productList
                .GroupJoin(orderList,
                p => p.ID,
                o => o.ProductID,
                (p, o) => new
                {
                    Prodotto = p.Name, //raggruppo per nome e dove non c'è match con ordini mette 0(perchè il valore di default di somma è 0)
                    Quantità = o.Sum(o => o.Quantity)
                });

            foreach (var item in groupJoinList)
            {
                Console.WriteLine("{0} - {1}", item.Prodotto, item.Quantità);
            }


            Console.WriteLine("GROUPJOIN: Query Syntax");

            var groupJoinList2 =
                from p in productList
                join o in orderList
                on p.ID equals o.ProductID
                into gr
                select new
                {
                    Prodotto = p.Name,
                    Quantità = gr.Sum(o => o.Quantity)
                };

            foreach (var item in groupJoinList2)
            {
                Console.WriteLine("{0} - {1}", item.Prodotto, item.Quantità);
            }

            //Lista nomeProdotto-Quantità: solo prodotti con ordini
            var lista4 =
                from o in orderList
                group o by o.ProductID
                into gr
                select new { ProdottoId = gr.Key, Quantità = gr.Sum(O => O.Quantity) }
                into gr1
                join p in productList
                on gr1.ProdottoId equals p.ID
                select new { Nome = p.Name, Quantità = gr1.Quantità };

            Console.WriteLine("Prodotti con ordini: ");
            foreach (var item in lista4)
            {
                Console.WriteLine("{0} - {1}", item.Nome, item.Quantità);
            }
        }
    }
}
