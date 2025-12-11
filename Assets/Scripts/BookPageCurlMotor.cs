using Assets.Scripts;
using Assets.Scripts.Componentes;
using Assets.Scripts.LIB;
using Assets.Scripts.Tipos;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static Assets.Scripts.Tipos.Conjuntos;


public class BookPageCurlMotor : MonoBehaviour {

    public Book book;

    public AutoFlip autoFlip;

    public PadraoObservadorAlvo observadoresAoConcluir = new PadraoObservadorAlvo();

    ProcessoMotor processoPagina = new ProcessoMotor();

    LivroJogoMotor livroJogoMotor;

    int idPaginaAtual;

    int idPaginaNova;

    VisualElement paginaPanilha;

    VisualElement paginaCampanha;



    void Awake() {
        livroJogoMotor = GetComponent<LivroJogoMotor>();
    }


    void Start() {
        paginaPanilha = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<VisualElement>("PaginaPanilha");
        paginaCampanha = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<VisualElement>("PaginaCampanha");
        processoPagina.monoBehaviour = this;
        processoPagina.rotinasIniciando.Add(RotinaProcesso_Pagina_Iniciando);
        processoPagina.rotinasConcluindo.Add(RotinaProcesso_Pagina_Concluido);
        processoPagina.rotinasFinalizado.Add(RotinaProcesso_Pagina_Finalizado);
    }


    IEnumerator RotinaProcesso_Pagina_Iniciando(Action<PROCESSO> aoAlterarProcesso) {
        aoAlterarProcesso?.Invoke(PROCESSO.INICIANDO_EXEC);
        if (paginaPanilha != null)
            paginaPanilha.style.display = Uteis.ObterDisplayStyle(false);
        if (paginaCampanha != null)
            paginaCampanha.style.display = Uteis.ObterDisplayStyle(false);
        yield return new WaitForSeconds(Constantes.TEMPO_ANIMACAO_PEQUENO);
        if (idPaginaAtual <= idPaginaNova) {
            if ((book.currentPage + 2) <= book.TotalPageCount)
                autoFlip.FlipRightPage();
            else
                autoFlip.FlipLeftPage();
        }
        else {
            if ((book.currentPage - 2) >= 0)
                autoFlip.FlipLeftPage();
            else
                autoFlip.FlipRightPage();
        }
        yield return new WaitForSeconds(Constantes.TEMPO_ANIMACAO_PEQUENO);
        aoAlterarProcesso?.Invoke(PROCESSO.PROCESSANDO);
    }


    IEnumerator RotinaProcesso_Pagina_Concluido(Action<PROCESSO> aoAlterarProcesso) {
        /*  yield return null;
            if (paginaPanilha != null)
                paginaPanilha.style.display = Uteis.ObterDisplayStyle(true);
            if (paginaCampanha != null)
                paginaCampanha.style.display = Uteis.ObterDisplayStyle(true);
        */
        observadoresAoConcluir.Notificar(OBSERVADOR_CONDICAO.PASSAR_PAGINA_DO_BOOK);
        aoAlterarProcesso?.Invoke(PROCESSO._FINALIZADO);
        yield return null;
    }


    IEnumerator RotinaProcesso_Pagina_Finalizado(Action<PROCESSO> aoAlterarProcesso) {
        aoAlterarProcesso?.Invoke(PROCESSO._ZERADO);
        LivroJogo.INSTANCIA.observadoresAlvos.Notificar(null);
        yield return null;
    }


    public void ImporPaginaAtual(int pagina) {
        if ((book is null) || (autoFlip is null))
            return;
        book.currentPage = pagina;
    }


    public void PassarPaginas(int idPaginaAtual, int idPaginaNova) {
        if ((book is null) || (autoFlip is null))
            return;
        if (processoPagina.processo != PROCESSO._ZERADO)
            return;
        this.idPaginaAtual = idPaginaAtual;
        this.idPaginaNova = idPaginaNova;
        book.OnFlip.AddListener(AoPassarPagina);
        processoPagina.Processar(PROCESSO.INICIANDO);
    }


    void AoPassarPagina() {
        processoPagina.Processar(PROCESSO.CONCLUINDO);
    }
}
