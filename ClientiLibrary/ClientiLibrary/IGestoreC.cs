namespace ClientiLibrary
{
    public interface IGestoreC
    {
        public void AggiungiCliente(Cliente nuovoCliente);
        List<Cliente> CercaCliente(string parametroRicerca, string scelta);
        void ModificaCliente(string id, Cliente clienteModificato);

        public bool EliminaCliente(string id);

    }
}
