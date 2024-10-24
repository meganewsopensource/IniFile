using System.Collections.Immutable;

namespace IniFile.Test.Models;

[IniSection("Pedido")]
internal class PedidoMock : IniFileSerializer
{ 
 
    
    [IniProperty("data")]
    private DateTime? data;
    
   [IniProperty("hora")]
    private DateTime? hora;

    [IniProperty("status")] 
    private StatusPedido status;
    
    [IniProperty("cliente")]
    private ClienteMock clienteMock;
    
    [IniProperty("produto")]
    private ProdutoMock produto;
    
    [IniProperty("itemPedido")]
    private IImmutableList<ItemPedidoMock> itens;
    
    [IniProperty("pagamento")]
    private IImmutableList<Pagamento> pagamentos;


    internal PedidoMock()
    {
        data = DateTime.Now;
        hora = DateTime.Now;
        clienteMock = new ClienteMock();
        produto = new ProdutoMock(
            1, "Farinha de trigo", 10.50d, 50d
        );

        status = StatusPedido.Faturado;
      
        itens = new List<ItemPedidoMock>()
        {
            new ItemPedidoMock(1, 12, 10),
            new ItemPedidoMock(1, 1, 50.55m),
            new ItemPedidoMock(1, 1.5D, 50.55m)
        }.ToImmutableList();

        pagamentos = new List<Pagamento>()
        {
            new Pagamento(FormaPagamento.Crediario, new List<Parcela>()
            {
                new Parcela(1, 15, DateTime.Now),
                new Parcela(2, 16, DateTime.Now)
            }),
            new Pagamento(FormaPagamento.CartaoCredito, new List<Parcela>()
            {
                new Parcela(1, 525.5f, DateTime.Now),
                new Parcela(2, 67.54f, DateTime.Now)
            }),
        }.ToImmutableList();
    }
    
}


internal class ItemPedidoMock(int idProduto, double quantidade, decimal precoVenda)
{

    [IniProperty("idProduto")]
    private int idProduto = idProduto;
    
    [IniProperty("qtd")]
    private double quantidade = quantidade;
    
    [IniProperty("prcoVenda")]
    private decimal precoVenda = precoVenda;
    
   
}


internal enum StatusPedido
{
    EmAberto = 100,
    AguardandoFaturamento = 200,
    Faturado = 300,
    AguardandoPagamento = 400,
    Concluido = 500,
    Cancelado = 600
}



internal class ProdutoMock(uint id, string descricao, double preco, double estoque)
{
    [IniProperty("codigo")]
    private uint id = id;
    
    [IniProperty("xDescr")]
    private string descricao = descricao;
    
    [IniProperty("pco")]
    private double preco = preco;
    
    [IniProperty("estoque")]
    private double estoque = estoque;
    
    
}

internal class ClienteMock
{
    [IniProperty("CPFCNPJ")]
    private string cpfCnpj;
    
    [IniProperty("xNome")]
    private string nome;
    
    [IniProperty("pessoa")]
    private TipoPessoa pessoa;

    internal ClienteMock()
    {
        this.cpfCnpj = "1234/56/789";
        this.nome = "John Doe";
        this.pessoa =  TipoPessoa.Fisica;
    }
    
}

internal enum TipoPessoa
{
    Fisica = 1,
    Juridica = 2
}

internal enum FormaPagamento
{
    Dinheiro,
    Pix,
    Crediario,
    CartaoCredito,
    CartaoDebito,
}

internal class Pagamento(FormaPagamento formaPagamento, IList<Parcela> parcelas)
{
    [IniProperty("formaPagamento")]
    private FormaPagamento formaPagamento = formaPagamento;
    [IniProperty("parcela")]
    private IList<Parcela> parcelas = parcelas;
}

internal class Parcela(int parcela, double valor, DateTime vencimento)
{
    [IniProperty("nroParcela")]
    private int parcela = parcela;
    [IniProperty("val")]
    private double valor = valor;
    [IniProperty("venc")]
    private DateTime vencimento = vencimento;
}

