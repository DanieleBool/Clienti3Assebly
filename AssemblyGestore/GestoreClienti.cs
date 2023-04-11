﻿using ClientiLibrary;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
//riferimenti database
using MySql.Data.MySqlClient;
using System.Configuration;


namespace AssemblyGestore
{
    public class GestoreClienti : IGestoreC
    {
        private string _connectionDB;

        // Costruttore che accetta il percorso come argomento
        public GestoreClienti(string connectionDB)
        {
            _connectionDB = connectionDB;
        }

        //public List<Cliente> CercaCliente(string parametroRicerca, string scelta)
        //{
        //    // Verifica che il parametro di ricerca non sia nullo o vuoto
        //    if (string.IsNullOrEmpty(parametroRicerca))
        //    {
        //        throw new ArgumentException("Il parametro di ricerca non può essere nullo o vuoto.");
        //    }

        //    // Verifica che il tipo di ricerca sia uno dei valori validi
        //    if (scelta != "ID" && scelta != "Nome" && scelta != "Cognome" && scelta != "Citta" && scelta != "Sesso" && scelta != "DataDiNascita")
        //    {
        //        throw new ArgumentException("Il tipo di ricerca non è valido.");
        //    }

        //    try
        //    {
        //        using (MySqlConnection conn = new MySqlConnection(parametroRicerca))
        //        {
        //            conn.Open();

        //            MySqlCommand command = new MySqlCommand();
        //            command.Connection = conn;

        //            // Costruisce la stringa di query in base al tipo di ricerca
        //            switch (scelta)
        //            {
        //                case "ID":
        //                    command.CommandText = "SELECT * FROM Clienti WHERE ID=@parametroRicerca";
        //                    command.Parameters.AddWithValue("@parametroRicerca", parametroRicerca);
        //                    break;
        //                case "Nome":
        //                    command.CommandText = "SELECT * FROM Clienti WHERE Nome=@parametroRicerca";
        //                    command.Parameters.AddWithValue("@parametroRicerca", parametroRicerca);
        //                    break;
        //                case "Cognome":
        //                    command.CommandText = "SELECT * FROM Clienti WHERE Cognome=@parametroRicerca";
        //                    command.Parameters.AddWithValue("@parametroRicerca", parametroRicerca);
        //                    break;
        //                case "Citta":
        //                    command.CommandText = "SELECT * FROM Clienti WHERE Citta=@parametroRicerca";
        //                    command.Parameters.AddWithValue("@parametroRicerca", parametroRicerca);
        //                    break;
        //                case "Sesso":
        //                    command.CommandText = "SELECT * FROM Clienti WHERE Sesso=@parametroRicerca";
        //                    command.Parameters.AddWithValue("@parametroRicerca", parametroRicerca);
        //                    break;
        //                case "DataDiNascita":
        //                    command.CommandText = "SELECT * FROM Clienti WHERE DataDiNascita=@parametroRicerca";
        //                    command.Parameters.AddWithValue("@parametroRicerca", Convert.ToDateTime(parametroRicerca));
        //                    break;
        //            }

        //            // Esegue la query e inserisce i risultati in una lista di clienti
        //            List<Cliente> clientiOut = new List<Cliente>();
        //            using (MySqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    string id = reader.GetString(0);
        //                    string nome = reader.GetString(1);
        //                    string cognome = reader.GetString(2);
        //                    string citta = reader.GetString(3);
        //                    string sesso = reader.GetString(4);
        //                    DateTime dataDiNascita = reader.GetDateTime(5);

        //                    Cliente cliente = new Cliente(id, nome, cognome, citta, sesso, dataDiNascita);
        //                    clientiOut.Add(cliente);
        //                }
        //            }

        //            return clientiOut;
        //        }
        //    }
        //    catch (MySqlException ex)
        //    {
        //        // Esegue il log dell'errore
        //        Console.WriteLine("Si è verificato un errore durante l'esecuzione della query:");
        //        Console.WriteLine(ex.Message);

        //        throw;
        //    }
        //}

        // CERCA //
        public List<Cliente> CercaCliente(string parametroRicerca, string scelta)
        {
            List<Cliente> clientiTrovati = new List<Cliente>();  // Crea una nuova lista vuota per memorizzare i clienti trovati

            // Verifica che il parametro di ricerca non sia nullo o vuoto
            if (string.IsNullOrEmpty(parametroRicerca))
            {
                throw new ArgumentException("Il parametro di ricerca non può essere vuoto.");
            }

            // Verifica che il tipo di ricerca sia uno dei valori validi
            if (scelta != "ID" && scelta != "Nome" && scelta != "Cognome" && scelta != "Citta" && scelta != "Sesso" && scelta != "DataDiNascita")
            {
                throw new ArgumentException("Il tipo di ricerca non è valido.");
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionDB))
                {
                    connection.Open();

                    string query = $"SELECT * FROM Clienti WHERE {scelta} = @parametroRicerca"; // Query SQL per cercare il cliente in base alla scelta dell'utente

                    // Crea un nuovo comando MySQL con la query e la connessione al database
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Imposta il valore del parametro nel comando
                        command.Parameters.AddWithValue("@parametroRicerca", parametroRicerca);

                        // Esegui la query e ottieni i risultati nell'oggetto MySqlDataReader 'reader'
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            // Leggi i risultati riga per riga
                            while (reader.Read())
                            {

                                // Crea un nuovo oggetto Cliente dai dati letti
                                Cliente cliente = new Cliente(
                                    reader.GetString("ID"),
                                    reader.GetString("Nome"),
                                    reader.GetString("Cognome"),
                                    reader.GetString("Citta"),
                                    reader.GetString("Sesso"),
                                    reader.GetDateTime("DataDiNascita"));

                                // Aggiungi il cliente trovato alla lista dei clienti trovati
                                clientiTrovati.Add(cliente);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // ex.Message restituisce solo il messaggio di errore dell'eccezione, mentre ex restituisce l'intera eccezione, compresi i dettagli
                throw new InvalidOperationException("Errore durante la ricerca dei clienti. Messaggio di errore: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                // Lancia un'eccezione con un messaggio personalizzato per tutti gli altri errori
                throw new InvalidOperationException("Si è verificato un errore durante la ricerca dei clienti. Messaggio di errore: " + ex.Message, ex);
            
            }
            
            // Restituisce la lista dei clienti trovati
            return clientiTrovati;
        }

        public void AggiungiCliente(Cliente nuovoCliente)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionDB))
                {
                    conn.Open();

                    // Controllo che l'ID non sia già presente nel database
                    MySqlCommand checkCmd = new MySqlCommand("SELECT COUNT(*) FROM Clienti WHERE ID = @ID", conn);
                    checkCmd.Parameters.AddWithValue("@ID", nuovoCliente.ID);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        throw new InvalidOperationException("Cliente già presente nel database.");
                    }

                    MySqlCommand cmd = new MySqlCommand("INSERT INTO Clienti (ID, Nome, Cognome, Citta, Sesso, DataDiNascita) VALUES (@ID, @Nome, @Cognome, @Citta, @Sesso, @DataDiNascita)", conn);

                    cmd.Parameters.Add("@ID", MySqlDbType.VarChar, 5).Value = nuovoCliente.ID;
                    cmd.Parameters.Add("@Nome", MySqlDbType.VarChar, 50).Value = nuovoCliente.Nome;
                    cmd.Parameters.Add("@Cognome", MySqlDbType.VarChar, 50).Value = nuovoCliente.Cognome;
                    cmd.Parameters.Add("@Citta", MySqlDbType.VarChar, 50).Value = nuovoCliente.Citta;
                    cmd.Parameters.Add("@Sesso", MySqlDbType.VarChar, 1).Value = nuovoCliente.Sesso;
                    cmd.Parameters.Add("@DataDiNascita", MySqlDbType.Date).Value = nuovoCliente.DataDiNascita;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException("Errore durante l'aggiunta del cliente. Messaggio di errore: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Si è verificato un errore sconosciuto durante l'aggiunta del cliente. Messaggio di errore: " + ex.Message, ex);
            }
        }

        //public void AggiungiCliente(Cliente cliente)
        //{
        //    // Crea la query SQL per inserire il nuovo cliente nel database
        //    string query = $"INSERT INTO Clienti (ID, Nome, Cognome, Citta, Sesso, DataDiNascita) " +
        //        $"VALUES ('{cliente.ID}', '{cliente.Nome}', '{cliente.Cognome}', '{cliente.Citta}', '{cliente.Sesso}', '{cliente.DataDiNascita.ToString("yyyy-MM-dd")}')";

        //    try
        //    {
        //        using (MySqlConnection conn = new MySqlConnection(_connectionDB))
        //        {
        //            conn.Open();

        //            using (MySqlCommand cmd = new MySqlCommand(query, conn))
        //            {
        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (MySqlException ex)
        //    {
        //        switch (ex.Number)
        //        {
        //            case 1042:
        //                throw new InvalidOperationException("Impossibile connettersi al database. Verifica la connessione e riprova.", ex);
        //            case 1062:
        //                throw new InvalidOperationException("Cliente già presente nel database.", ex);
        //            default:
        //                throw new InvalidOperationException("Errore durante l'inserimento del cliente nel database.", ex);
        //        }
        //    }
        //}

        //public void AggiungiCliente(Cliente nuovoCliente)
        //{
        //    try
        //    {
        //        using (MySqlConnection connection = new MySqlConnection(_connectionDB))
        //        {
        //            connection.Open();

        //            // Query per cercare un cliente con lo stesso ID
        //            string query = "SELECT COUNT(*) FROM Clienti WHERE ID = @ID";

        //            // Dichiarazione di un comando con la query e la connessione al database
        //            using (MySqlCommand command = new MySqlCommand(query, connection))
        //            {
        //                // Impostazione del valore del parametro "@ID" nel comando
        //                command.Parameters.AddWithValue("@ID", nuovoCliente.ID);

        //                // Esecuzione della query e ottenimento del risultato in count
        //                int count = Convert.ToInt32(command.ExecuteScalar());

        //                if (count > 0) // Controllo se esiste già un cliente con lo stesso ID nel database
        //                {
        //                    // Se esiste, lancio un'eccezione con un messaggio di errore
        //                    throw new InvalidOperationException("L'ID del cliente esiste già nel database.");
        //                }
        //            }

        //            // Dichiarazione della query per inserire il nuovo cliente
        //            string insertQuery = "INSERT INTO Clienti (ID, Nome, Cognome, Citta, Sesso, DataDiNascita) " + "VALUES (@ID, @Nome, @Cognome, @Citta, @Sesso, @DataDiNascita)";

        //            // Dichiarazione di un comando con la query e la connessione al database
        //            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
        //            {
        //                // Impostazione dei valori dei parametri nel comando
        //                insertCommand.Parameters.Add("@ID", MySqlDbType.VarChar, 5).Value = nuovoCliente.ID;
        //                insertCommand.Parameters.Add("@Nome", MySqlDbType.VarChar, 50).Value = nuovoCliente.Nome;
        //                insertCommand.Parameters.Add("@Cognome", MySqlDbType.VarChar, 50).Value = nuovoCliente.Cognome;
        //                insertCommand.Parameters.Add("@Citta", MySqlDbType.VarChar, 50).Value = nuovoCliente.Citta;
        //                insertCommand.Parameters.Add("@Sesso", MySqlDbType.VarChar, 1).Value = nuovoCliente.Sesso;
        //                insertCommand.Parameters.Add("@DataDiNascita", MySqlDbType.Date).Value = nuovoCliente.DataDiNascita;

        //                // Esecuzione della query di inserimento e agginta del valore a rowsAffected
        //                int rowsAffected = insertCommand.ExecuteNonQuery();

        //                // Controllo quante righe ha inserito la query, deve essere 1
        //                if (rowsAffected != 1)
        //                {
        //                    throw new InvalidOperationException("Errore durante l'inserimento del nuovo cliente.");
        //                }
        //            }
        //        }
        //    }
        //    catch (MySqlException ex)
        //    {
        //        // Lancia un'eccezione con un messaggio personalizzato per gli errori di MySQL
        //        throw new InvalidOperationException("Errore durante l'aggiunta del cliente. Messaggio di errore: " + ex.Message, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Lancia un'eccezione con un messaggio personalizzato per tutti gli altri errori
        //        throw new InvalidOperationException("Si è verificato un errore durante l'aggiunta del cliente. Messaggio di errore: " + ex.Message, ex);
        //    }
        //}


        //// CERCA //
        //public List<Cliente> CercaCliente(string parametroRicerca, string scelta)
        //{
        //    List<Cliente> clientiTrovati = new List<Cliente>();  // Crea una nuova lista vuota per memorizzare i clienti trovati

        //    try
        //    {
        //        using (MySqlConnection connection = new MySqlConnection(_connectionDB))
        //        {
        //            connection.Open();

        //            string query = $"SELECT * FROM Clienti WHERE {scelta} = @parametroRicerca"; // Query SQL per cercare il cliente in base alla scelta dell'utente

        //            // Crea un nuovo comando MySQL con la query e la connessione al database
        //            using (MySqlCommand command = new MySqlCommand(query, connection))
        //            {
        //                // Imposta il valore del parametro nel comando
        //                command.Parameters.AddWithValue("@parametroRicerca", parametroRicerca);

        //                // Esegui la query e ottieni i risultati nell'oggetto MySqlDataReader 'reader'
        //                using (MySqlDataReader reader = command.ExecuteReader())
        //                {
        //                    // Leggi i risultati riga per riga
        //                    while (reader.Read())
        //                    {

        //                        // Crea un nuovo oggetto Cliente dai dati letti
        //                        Cliente cliente = new Cliente(
        //                            reader.GetString("ID"),
        //                            reader.GetString("Nome"),
        //                            reader.GetString("Cognome"),
        //                            reader.GetString("Citta"),
        //                            reader.GetString("Sesso"),
        //                            reader.GetDateTime("DataDiNascita"));

        //                        // Aggiungi il cliente trovato alla lista dei clienti trovati
        //                        clientiTrovati.Add(cliente);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        // Lancia un'eccezione InvalidOperationException con un messaggio personalizzato
        //        throw new InvalidOperationException("Errore durante la ricerca dei clienti.", ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Lancia un'eccezione InvalidOperationException con un messaggio personalizzato
        //        throw new InvalidOperationException("Non hai inserito un input valido.", ex);
        //    }

        //    // Restituisce la lista dei clienti trovati
        //    return clientiTrovati;
        //}

        //public void AggiungiCliente(Cliente nuovoCliente)
        //{
        //    //try
        //    //{
        //        using (MySqlConnection connection = new MySqlConnection(_connectionDB))
        //        {
        //            connection.Open();

        //            // Query per cercare un cliente con lo stesso ID
        //            string query = "SELECT COUNT(*) FROM Clienti WHERE ID = @ID";

        //            // Dichiarazione di un comando con la query e la connessione al database
        //            using (MySqlCommand command = new MySqlCommand(query, connection))
        //            {
        //                // Impostazione del valore del parametro "@ID" nel comando
        //                command.Parameters.AddWithValue("@ID", nuovoCliente.ID);

        //                // Uso ToInt32 perchè ExecuteScalar() fa partire il command e restituisce un oggetto, io ho bisogno di un numero per il conteggio - count conterrà il risutato del "COUNT"
        //                // (ExecuteScalar() prende il primo valore della prima riga del set di risultati, ExecuteReader() se avessi avuto più di un risultato) la query restituisce solo un valore poiché uso la funzione di aggregazione "COUNT" (conteggia le righe che soddisfano la condizione)
        //                int count = Convert.ToInt32(command.ExecuteScalar());

        //                if (count > 0) // Controllo se esiste già un cliente con lo stesso ID nel database
        //                {
        //                    // Se esiste, lancio un'eccezione con un messaggio di errore
        //                    throw new InvalidOperationException("L'ID del cliente esiste già nel database.");
        //                }
        //            }

        //            // Dichiarazione della query per inserire il nuovo cliente
        //            string insertQuery = "INSERT INTO Clienti (ID, Nome, Cognome, Citta, Sesso, DataDiNascita) " + "VALUES (@ID, @Nome, @Cognome, @Citta, @Sesso, @DataDiNascita)";

        //            // Dichiarazione di un comando con la query e la connessione al database
        //            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
        //            {
        //                // Impostazione dei valori dei parametri nel comando
        //                insertCommand.Parameters.Add("@ID", MySqlDbType.VarChar, 5).Value = nuovoCliente.ID; ;
        //                insertCommand.Parameters.Add("@Nome", MySqlDbType.VarChar, 50).Value = nuovoCliente.Nome;
        //                insertCommand.Parameters.Add("@Cognome", MySqlDbType.VarChar, 50).Value = nuovoCliente.Cognome;
        //                insertCommand.Parameters.Add("@Citta", MySqlDbType.VarChar, 50).Value = nuovoCliente.Citta;
        //                insertCommand.Parameters.Add("@Sesso", MySqlDbType.VarChar, 1).Value = nuovoCliente.Sesso;
        //                insertCommand.Parameters.Add("@DataDiNascita", MySqlDbType.Date).Value = nuovoCliente.DataDiNascita;

        //                // Esecuzione della query di inserimento e agginta del valore a rowsAffected
        //                int rowsAffected = insertCommand.ExecuteNonQuery();

        //                // Controllo quante righe ha inserito la query, deve essere 1
        //                if (rowsAffected != 1)
        //                {
        //                    throw new InvalidOperationException("Errore durante l'inserimento del nuovo cliente.");
        //                }
        //            }
        //        }
        //    //}catch(Exception ex)
        //    //{
        //    //    throw new InvalidOperationException("Errore", ex);
        //    //}
        //}

        // MODIFICA //
        public void ModificaCliente(string id, Cliente clienteModificato) //in input i dati da modificare (clienteModificato)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionDB))
                {
                    conn.Open();

                    MySqlCommand checkCmd = new MySqlCommand("SELECT COUNT(*) FROM Clienti WHERE ID = @ID", conn);
                    checkCmd.Parameters.AddWithValue("@ID", id);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count == 0)
                    {
                        throw new InvalidOperationException("Il cliente con l'ID specificato non esiste nel database.");
                    }

                    MySqlCommand cmd = new MySqlCommand("UPDATE Clienti SET Nome = @Nome, Cognome = @Cognome, Citta = @Citta, Sesso = @Sesso, DataDiNascita = @DataDiNascita WHERE ID = @ID", conn);

                    cmd.Parameters.Add("@ID", MySqlDbType.VarChar, 5).Value = id;
                    cmd.Parameters.Add("@Nome", MySqlDbType.VarChar, 50).Value = clienteModificato.Nome;
                    cmd.Parameters.Add("@Cognome", MySqlDbType.VarChar, 50).Value = clienteModificato.Cognome;
                    cmd.Parameters.Add("@Citta", MySqlDbType.VarChar, 50).Value = clienteModificato.Citta;
                    cmd.Parameters.Add("@Sesso", MySqlDbType.VarChar, 1).Value = clienteModificato.Sesso;
                    cmd.Parameters.Add("@DataDiNascita", MySqlDbType.Date).Value = clienteModificato.DataDiNascita;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException("Modifica del cliente non riuscita.", ex);
            }
            // ECCO COME RICHIAMARE L'ECCEZIONE DEL'IF (COUNT == 0)
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Si è verificato un errore sconosciuto durante la modifica del cliente.", ex);
            }
        }

        // ELIMINA //
        public bool EliminaCliente(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionDB))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM Clienti WHERE ID = @ID", conn);
                    cmd.Parameters.Add("@ID", MySqlDbType.VarChar, 5).Value = id;
                    // ExecuteNonQuery() restituisce il numero di righe interessate dalla query non dei dati, quindi lo associo a rowsAffected
                    int rowsAffected = cmd.ExecuteNonQuery();
                    // Controllo se la query ha eliminato almeno una riga
                    return rowsAffected > 0;
                }
            }
            catch (MySqlException ex)
            {
                //", ex" serve per stampare il messaggio di errore predefinito di MySqlException e capire il vero errore
                throw new InvalidOperationException("Errore durante l'eliminazione del cliente.", ex);
                //return false; // non serve più se c'è InvalidOperationException
            }
            catch (Exception ex)
            {
                // Lancia un'eccezione InvalidOperationException con un messaggio personalizzato
                throw new InvalidOperationException("Non hai inserito un input valido.", ex);
            }
        }


        public bool VerificaIDUnivoco(string id)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionDB))
            {
                connection.Open();

                // Selezionara tutti gli ID dal database
                string query = "SELECT ID FROM clienti";

                // Crea un oggetto MySqlCommand, passando la query e la connessione al db (procedura standard)
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Legge gli ID dal database utilizzando il metodo ExecuteReader del command (MySqlCommand)
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Controlla se l'ID cercato esiste già nella lista degli ID letti dal database
                        //Il metodo Read() sposta il cursore del lettore sulla riga successiva del risultato, ritornando true se ci sono altre righe disponibili.
                        while (reader.Read())
                        {
                            // GetString mi serve per leggere il valore della riga che cambierà di continuo grazie al while
                            if (id == reader.GetString(0)) // Lo 0 serve per indicare che deve leggere le rihe della colonna 0
                            {
                                return false; // L'ID non è univoco
                            }
                        }
                    }
                }
            }
            return true; // L'ID è univoco
        }
    }
}