// See https://aka.ms/new-console-template for more information
using ClientiLibrary;
using AssemblyGestore;
using System;
using System.Globalization;
using System.IO;
using System.Configuration;
using MySql.Data.MySqlClient;

class Program
{
    static void Main(string[] args)
    {
        // Leggi la stringa di connessione da app.config
        string connectionDB = ConfigurationManager.AppSettings["DefaultConnection"];

        // Crea un'istanza di GestoreClienti e passa la stringa di connessione
        IGestoreC gestore = new GestoreClienti(connectionDB);

        while (true)
        {
            Console.WriteLine("Scegli un'opzione:");
            Console.WriteLine("1. Cerca cliente");
            Console.WriteLine("2. Aggiungi cliente");
            Console.WriteLine("3. Modifica cliente");
            Console.WriteLine("4. Elimina cliente");
            Console.Write("Inserisci il numero dell'opzione: ");
            //controllo input dell'opzione, legge l'input e lo converte in intero (la conversone in intero mi serve per dei controlli sulla scelta dell'opzione)
            int.TryParse(Console.ReadLine(), out int opzione);

            switch (opzione)
            {

                // CERCA CLIENTE //
                case 1:
                    try
                    {
                        Console.WriteLine("Scegli l'informazione da cercare:");
                        Console.WriteLine("1. ID");
                        Console.WriteLine("2. Nome");
                        Console.WriteLine("3. Cognome");
                        Console.WriteLine("4. Città");
                        Console.WriteLine("5. Sesso");
                        Console.WriteLine("6. Data di Nascita");

                        string scelta;
                        int sceltaInt;
                        do
                        {
                            scelta = Console.ReadLine();
                            int.TryParse(scelta, out sceltaInt);
                            if (sceltaInt < 1 || sceltaInt > 6)
                            {
                                Console.WriteLine("Inserisci un numero tra 1 e 6:");
                            }
                        } while (sceltaInt < 1 || sceltaInt > 6);

                        switch (scelta)
                        {
                            case "1":
                                scelta = "ID";
                                break;
                            case "2":
                                scelta = "Nome";
                                break;
                            case "3":
                                scelta = "Cognome";
                                break;
                            case "4":
                                scelta = "Citta";
                                break;
                            case "5":
                                scelta = "Sesso";
                                break;
                            case "6":
                                scelta = "DataDiNascita";
                                break;
                        }

                        Console.WriteLine("Scrivi l'informazione da cercare:");
                        string parametroRicerca = Console.ReadLine();

                        List<Cliente> clientiOut = gestore.CercaCliente(parametroRicerca, scelta);

                        if (clientiOut.Count > 0)
                        {
                            Console.WriteLine("Clienti trovati:");
                            foreach (Cliente cliente in clientiOut)
                            {
                                Console.WriteLine(cliente.ToRead());
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nessun cliente trovato.");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine("Errore durante la ricerca del cliente: " + ex.Message);
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine($"Errore: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Errore generico: {ex.Message}");
                    }
                    break;

                //// CERCA CLIENTE //
                //     case 1:
                //         try
                //         {
                //             Console.WriteLine("Scegli l'informazione da cercare:");
                //             Console.WriteLine("1. ID");
                //             Console.WriteLine("2. Nome");
                //             Console.WriteLine("3. Cognome");
                //             Console.WriteLine("4. Città");
                //             Console.WriteLine("5. Sesso");
                //             Console.WriteLine("6. Data di Nascita");


                //             string scelta;
                //             int sceltaInt;
                //             do
                //             {
                //                 scelta = Console.ReadLine(); // Legge la scelta dell'utente dall'input della console (scelta che poi passerò al metodo)

                //                 // Non posso convertire la stringa scelta in un int per questo motivo devo creare la variabile sceltaInt con cui confrontare
                //                 int.TryParse(scelta, out sceltaInt); 
                //                 if (sceltaInt < 1 || sceltaInt > 6)
                //                 {
                //                     Console.WriteLine("Inserisci un numero tra 1 e 6:");
                //                 }
                //             } while (sceltaInt < 1 || sceltaInt > 6); // Ripete il ciclo finché l'input dell'utente non è un numero valido tra 1 e 6

                //             //imposta il tipo di ricerca in base all'input dell'utente
                //             switch (scelta)
                //             {
                //                 case "1":
                //                     scelta = "ID";
                //                     break;
                //                 case "2":
                //                     scelta = "Nome";
                //                     break;
                //                 case "3":
                //                     scelta = "Cognome";
                //                     break;
                //                 case "4":
                //                     scelta = "Citta";
                //                     break;
                //                 case "5":
                //                     scelta = "Sesso";
                //                     break;
                //                 case "6":
                //                     scelta = "DataDiNascita";
                //                     break;
                //             }

                //             Console.WriteLine("Scrivi l'informazione da cercare:");
                //             string parametroRicerca = Console.ReadLine();

                //             // Chiama il metodo CercaCliente ed inserisce il risultato nella lista "clientiOut"
                //             List<Cliente> clientiOut = gestore.CercaCliente(parametroRicerca, scelta);

                //             if (clientiOut.Count > 0) // Controlla se la lista dei clienti trovati non è vuota
                //             {
                //                 Console.WriteLine("Clienti trovati:");
                //                 // Stampa le informazioni di ogni cliente trovato nella lista
                //                 foreach (Cliente cliente in clientiOut)
                //                 {
                //                     Console.WriteLine(cliente.ToRead());
                //                 }
                //             }
                //             else
                //             {
                //                 Console.WriteLine("Nessun cliente trovato."); // Se la lista dei clientiOut è vuota
                //             }
                //         }
                //         catch (InvalidOperationException ex)
                //         {
                //             Console.WriteLine(ex.Message);
                //         }
                //         break;

                // AGGIUNGI CLIENTE //
                case 2:
                    bool continuaAdAggiungereClienti = true;
                    while (continuaAdAggiungereClienti)
                    {
                        try
                        {
                            Console.Write("Inserisci l'ID del cliente: ");
                            string id = Console.ReadLine();
                            if ((id.Length < 1)|| (id.Length > 5) || (id == null))
                            {
                                Console.WriteLine("L'ID del cliente deve essere di 5 caratteri.");
                                break;
                            }

                            Console.Write("Inserisci il nome del cliente: ");
                            string nome = Console.ReadLine();
                            if (string.IsNullOrEmpty(nome))
                            {
                                Console.WriteLine("Il nome del cliente non può essere vuoto.");
                                break;
                            }

                            Console.Write("Inserisci il cognome del cliente: ");
                            string cognome = Console.ReadLine();
                            if (string.IsNullOrEmpty(cognome))
                            {
                                Console.WriteLine("Il nome del cliente non può essere vuoto.");
                                break;
                            }

                            Console.Write("Inserisci la città del cliente: ");
                            string citta = Console.ReadLine();
                            if (string.IsNullOrEmpty(citta))
                            {
                                Console.WriteLine("Il nome del cliente non può essere vuoto.");
                                break;
                            }

                            string sesso = string.Empty;
                            while (string.IsNullOrEmpty(sesso))
                            {
                                Console.Write("Inserisci il sesso del cliente (M/F): ");
                                string sessoInput = Console.ReadLine().ToUpper();
                                if (sessoInput == "M" || sessoInput == "F")
                                {
                                    sesso = sessoInput;
                                }
                                else
                                {
                                    Console.WriteLine("Inserimento non valido. Inserisci 'M' o 'F'.");
                                }
                            }

                            DateTime dataDiNascita;
                            while (true)
                            {
                                Console.Write("Inserisci la data di nascita del cliente (formato: dd/MM/yyyy): ");
                                string dataInserita = Console.ReadLine();

                                //if (DateTime.TryParseExact(dataInserita, new[] { "ddMMyyyy", "dd/MM/yyyy", "dd-MM-yyyy" },
                                //    CultureInfo.InvariantCulture, DateTimeStyles.None, out dataDiNascita))
                                //{
                                //    break;
                                //}
                                //else
                                //{
                                //    Console.WriteLine("Formato data non valido. Riprova.");
                                //}
                                string[] formatiData = { "dd/MM/yyyy", "dd-MM-yyyy", "yyyyMMdd" };
                                //L'array formatiData contiene i formati che verranno provati uno alla volta per verificare se la stringa inserita dall'utente corrisponde a uno di essi, metterdo dopo la irgola fa parte di DateTime.TryParseExact
                                if (!DateTime.TryParseExact(dataInserita, formatiData, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataDiNascita))
                                {
                                    Console.WriteLine("Formato data non valido. Riprova.");
                                }
                                else
                                {
                                    break;
                                }

                            }

                            Cliente nuovoCliente = new Cliente(id, nome, cognome, citta, sesso, dataDiNascita);

                            gestore.AggiungiCliente(nuovoCliente);
                            Console.WriteLine("Cliente aggiunto con successo.");

                            // Chiedi all'utente se vuole continuare ad aggiungere clienti
                            Console.WriteLine("Premi \"Invio\" per aggiungere un'altro cliente o \"N\" per uscire ");
                            string continua = Console.ReadLine().ToUpper();
                            if (continua == "N")
                            {
                                continuaAdAggiungereClienti = false;
                            }
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine("Errore: " + ex.Message);
                            Console.WriteLine("Inserimento non riuscito. Riprova.");
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Errore: Formato input non valido. Riprova.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Errore generico: " + ex.Message);
                        }
                    }
                    break;

                //// AGGIUNGI CLIENTE //
                //case 2:
                //    // Continua ad aggiungere clienti finché l'utente decide di fermarsi
                //    bool continuaAdAggiungereClienti = true;
                //    while (continuaAdAggiungereClienti)
                //    {
                //        Console.Write("Inserisci l'ID del cliente: ");
                //        string id = Console.ReadLine();

                //        Console.Write("Inserisci il nome del cliente: ");
                //        string nome = Console.ReadLine();

                //        Console.Write("Inserisci il cognome del cliente: ");
                //        string cognome = Console.ReadLine();

                //        Console.Write("Inserisci la città del cliente: ");
                //        string citta = Console.ReadLine();

                //        // Inserisci il sesso del cliente e verifica che sia un input valido
                //        string sesso = string.Empty;
                //        while (string.IsNullOrEmpty(sesso))
                //        {
                //            Console.Write("Inserisci il sesso del cliente (M/F): ");
                //            string sessoInput = Console.ReadLine().ToUpper();
                //            if (sessoInput == "M" || sessoInput == "F")
                //            {
                //                sesso = sessoInput;
                //            }
                //            else
                //            {
                //                Console.WriteLine("Inserimento non valido. Inserisci 'M' o 'F'.");
                //            }
                //        }

                //        // Inserisci la data di nascita del cliente e verifica che sia un input valido
                //        DateTime dataDiNascita;
                //        while (true)
                //        {
                //            Console.Write("Inserisci la data di nascita del cliente (formato: dd/MM/yyyy): ");
                //            string dataInserita = Console.ReadLine();

                //            // Tenta di convertire la data inserita in un oggetto DateTime
                //            if (DateTime.TryParseExact(dataInserita, new[] { "ddMMyyyy", "dd/MM/yyyy", "dd-MM-yyyy" },
                //                CultureInfo.InvariantCulture, DateTimeStyles.None, out dataDiNascita))
                //            {
                //                // Se la conversione ha successo, esci dal ciclo 'while'
                //                break;
                //            }
                //            else
                //            {
                //                Console.WriteLine("Formato data non valido. Riprova.");
                //            }
                //        }

                //        // Crea un nuovo oggetto Cliente con i dettagli forniti
                //        Cliente nuovoCliente = new Cliente(id, nome, cognome, citta, sesso, dataDiNascita);

                //        // Tenta di aggiungere il nuovo cliente al database
                //        bool clienteAggiunto = false;
                //        while (!clienteAggiunto)
                //        {
                //            try
                //            {
                //                gestore.AggiungiCliente(nuovoCliente);
                //                Console.WriteLine("Cliente aggiunto con successo.");
                //                clienteAggiunto = true;
                //            }
                //            catch (InvalidOperationException ex)
                //            {
                //                Console.WriteLine(ex.Message);
                //                Console.WriteLine("Inserisci un nuovo ID per il cliente:");
                //                nuovoCliente.ID = Console.ReadLine();
                //            }
                //        }

                //        // Chiedi all'utente se vuole continuare ad aggiungere clienti
                //        Console.WriteLine("Premi \"Invio\" per aggiungere un'altro cliente o \"N\" per uscire ");
                //        string continua = Console.ReadLine().ToUpper();
                //        if (continua == "N")
                //        {
                //            continuaAdAggiungereClienti = false;
                //        }
                //    }
                //    break;

                // MODIFICA CLIENTE //
                case 3: 
                    Console.Write("Inserisci l'ID del cliente da modificare: ");
                    string idCliente = Console.ReadLine();

                    // Tramite Find  estraggo da CercaCliente il cliente con l'ID corrispondente alla ricerca(quindi lo estraggo ma non lo leggo in console come nel case1)
                    // crca nel file tramite il metodo CercaCliente con il paramentro di scelta inpostato su "ID" e usa FirstOrDefault() per estrarre il primo oggetto Cliente trovato nel file o null se la lista è vuota.
                    Cliente clienteDaModificare = gestore.CercaCliente(idCliente, "ID").Find(cliente => cliente.ID == idCliente); //.FirstOrDefault();

                    if (clienteDaModificare == null)
                    {
                        //Console.WriteLine("Cliente non trovato.");
                        //break;
                    }
                    else
                    {
                        Console.WriteLine("Inserisci le nuove informazioni del cliente o premi Invio per mantenere le informazioni attuali:");

                        Console.Write($"Inserisci il nuovo nome del cliente ({clienteDaModificare.Nome}): ");
                        string nuovoNome = Console.ReadLine();
                        if (string.IsNullOrEmpty(nuovoNome))
                        {
                            nuovoNome = clienteDaModificare.Nome;
                        }

                        Console.Write($"Inserisci il nuovo cognome del cliente ({clienteDaModificare.Cognome}): ");
                        string nuovoCognome = Console.ReadLine();
                        if (string.IsNullOrEmpty(nuovoCognome))
                        {
                            nuovoCognome = clienteDaModificare.Cognome;
                        }

                        Console.Write($"Inserisci la nuova città del cliente ({clienteDaModificare.Citta}): ");
                        string nuovaCitta = Console.ReadLine();
                        if (string.IsNullOrEmpty(nuovaCitta))
                        {
                            nuovaCitta = clienteDaModificare.Citta;
                        }

                        //SESSO
                        string nuovoSesso = clienteDaModificare.Sesso; // nuovoSesso sarà uguale al sesso originale, in questo modo non cambia se non nell'"else if"
                        while (true)
                        {
                            Console.Write($"Inserisci il nuovo sesso del cliente ({clienteDaModificare.Sesso}): ");
                            string inputSesso = Console.ReadLine().ToUpper();

                            if (string.IsNullOrEmpty(inputSesso)) // se l'input è vuoto mantiene l'info esistente
                            {
                                break;
                            }
                            else if (inputSesso == "M" || inputSesso == "F")
                            {
                                nuovoSesso = inputSesso; // Aggiorna il valore di nuovoSesso solo se l'input è valido
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Sesso non valido. Inserisci 'M' o 'F'.");
                            }
                        }

                        //DATA
                        Console.Write($"Inserisci la nuova data di nascita del cliente ({clienteDaModificare.DataDiNascita:dd/MM/yyyy}): ");
                        string nuovaDataInserita = Console.ReadLine();
                        // trasforma la stringa in data e la aggiunge al nuovo oggetto
                        DateTime nuovaDataDiNascita = clienteDaModificare.DataDiNascita;
                        // se la data è vuota
                        if (!string.IsNullOrEmpty(nuovaDataInserita))
                        {
                            if (!DateTime.TryParseExact(nuovaDataInserita, new[] { "ddMMyyyy", "dd/MM/yyyy" },
                                CultureInfo.InvariantCulture, DateTimeStyles.None, out nuovaDataDiNascita))
                            {
                                Console.WriteLine("Formato data non valido. Riprova.");
                                return;
                            }
                        }
                        // Creo il nuovo oggetto
                        Cliente clienteModificato = new Cliente(idCliente, nuovoNome, nuovoCognome, nuovaCitta, nuovoSesso, nuovaDataDiNascita);

                        // Eccezioni
                        try
                        {
                            gestore.ModificaCliente(idCliente, clienteModificato); // Si avvia il metodo
                            Console.WriteLine("Cliente modificato con successo.");
                        }
                        catch (InvalidOperationException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        catch (MySqlException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    break;
                    

               // ELIMINA CLIENTE //
                case 4:
                    Console.WriteLine("Inserisci l'ID del cliente da eliminare");
                    string outID = Console.ReadLine();
                    try
                    {
                        // Se il Metodo restituisce true, "eliminato" sarà true e quindi non si passerà per il catch
                        bool eliminato = gestore.EliminaCliente(outID);
                        Console.WriteLine("Cliente eliminato con successo.");
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;

                default:
                    Console.WriteLine("Opzione non valida. Riprova.");
                    break;
            }
        }
    }
}





















//case 3:
//    static string RichiediInformazione(string messaggio, string valoreDefault)
//    {
//        Console.Write($"{messaggio} ({valoreDefault}): ");
//        string valoreInserito = Console.ReadLine();
//        return string.IsNullOrEmpty(valoreInserito) ? valoreDefault : valoreInserito;
//    }


//    Console.Write("Inserisci l'ID del cliente da modificare: ");
//    string idCliente = Console.ReadLine();

//    Cliente clienteDaModificare = gestore.CercaCliente(idCliente, "ID").FirstOrDefault();

//    if (clienteDaModificare == null)
//    {
//        Console.WriteLine("Cliente non trovato.");
//        break;
//    }
//    else
//    {
//        Console.WriteLine("Inserisci le nuove informazioni del cliente o premi Invio per mantenere le informazioni attuali:");

//        string nuovoNome = RichiediInformazione("Inserisci il nuovo nome del cliente", clienteDaModificare.Nome);
//        string nuovoCognome = RichiediInformazione("Inserisci il nuovo cognome del cliente", clienteDaModificare.Cognome);
//        string nuovaCitta = RichiediInformazione("Inserisci la nuova città del cliente", clienteDaModificare.Citta);

//        string nuovoSesso = RichiediInformazione("Inserisci il nuovo sesso del cliente", clienteDaModificare.Sesso).ToUpper();
//        while (nuovoSesso != "M" && nuovoSesso != "F")
//        {
//            Console.WriteLine("Sesso non valido. Inserisci 'M' o 'F'.");
//            nuovoSesso = RichiediInformazione("Inserisci il nuovo sesso del cliente", clienteDaModificare.Sesso).ToUpper();
//        }

//        string nuovaDataInserita = RichiediInformazione("Inserisci la nuova data di nascita del cliente", clienteDaModificare.DataDiNascita.ToString("dd/MM/yyyy"));
//        DateTime nuovaDataDiNascita = clienteDaModificare.DataDiNascita;
//        while (!string.IsNullOrEmpty(nuovaDataInserita) && !DateTime.TryParseExact(nuovaDataInserita, new[] { "ddMMyyyy", "dd/MM/yyyy" },
//                    CultureInfo.InvariantCulture, DateTimeStyles.None, out nuovaDataDiNascita))
//        {
//            Console.WriteLine("Formato data non valido. Riprova.");
//            nuovaDataInserita = RichiediInformazione("Inserisci la nuova data di nascita del cliente", clienteDaModificare.DataDiNascita.ToString("dd/MM/yyyy"));
//        }

//        Cliente clienteModificato = new Cliente(idCliente, nuovoNome, nuovoCognome, nuovaCitta, nuovoSesso, nuovaDataDiNascita);
//        gestore.ModificaCliente(idCliente, clienteModificato);
//        Console.WriteLine("Cliente modificato con successo.");

//        break;
//    }
//default:
//    Console.WriteLine("Opzione non valida. Riprova.");
//    break;

//            }
//        }
//    }
//}

////dichiaro la varibile conn di tipo MySqlConnection (rappresenta la connesione al db)
//MySql.Data.MySqlClient.MySqlConnection conn;

//string connectionDB = "server=127.0.0.1;uid=root;" + "pwd=Kondor99$;port=3306;database=clienti_db";

//try
//{
//    //nuova istanza della classe MySqlConnection e la assegna alla variabile conn (dichiarata sopra)
//    conn = new MySql.Data.MySqlClient.MySqlConnection();
//    conn.ConnectionString = connectionDB;
//    conn.Open();
//}
////ex è una variabile di tipo MySql.Data.MySqlClient.MySqlException, è un'eccezione generata dalla libreria MySQL quando si verifica un errore, può essere utilizzata per avere maggiori info sull'eccezione
//catch (MySql.Data.MySqlClient.MySqlException ex)
//{
//    Console.WriteLine("Connessione non riuscita.");
//}

////string connectionDB = "Server=localhost;Database=clienti_db;port=3306;Uid=root;Pwd=;";
//IGestoreC gestore = new GestoreClienti(connectionDB);