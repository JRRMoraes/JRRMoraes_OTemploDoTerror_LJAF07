using Assets.Scripts;
using Assets.Scripts.Componentes;
using Assets.Scripts.LIB;
using Assets.Scripts.Tipos;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static Assets.Scripts.Tipos.Conjuntos;


public class BookPageCurlMotor : MonoBehaviour, IPadraoObservador {

    public Book book;

    public AutoFlip autoFlip;

    ProcessoMotorIEnumerator processoPagina = new ProcessoMotorIEnumerator();

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


    public void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao) {
        if (observadorCondicao != OBSERVADOR_CONDICAO.PASSAR_PAGINA_DO_BOOK)
            return;
        if (!LivroJogoMotor.EhValido(livroJogoMotor))
            return;

        processoPagina.Processar();
    }


    IEnumerator RotinaProcesso_Pagina_Iniciando(Action<PROCESSO, bool> aoAlterarProcesso) {
        yield return null;
        if (paginaPanilha != null)
            paginaPanilha.style.visibility = Uteis.ObterVisibility(false);
        if (paginaCampanha != null)
            paginaCampanha.style.visibility = Uteis.ObterVisibility(false);
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
        aoAlterarProcesso?.Invoke(PROCESSO.PROCESSANDO, true);
    }


    IEnumerator RotinaProcesso_Pagina_Concluido(Action<PROCESSO, bool> aoAlterarProcesso) {
        /*  yield return null;
            if (paginaPanilha != null)
                paginaPanilha.style.visibility = Uteis.ObterVisibility(true);
            if (paginaCampanha != null)
                paginaCampanha.style.visibility = Uteis.ObterVisibility(true);
        */
        aoAlterarProcesso?.Invoke(PROCESSO._FINALIZADO, true);
        yield return null;
    }


    IEnumerator RotinaProcesso_Pagina_Finalizado(Action<PROCESSO, bool> aoAlterarProcesso) {
        aoAlterarProcesso?.Invoke(PROCESSO._ZERADO, false);
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
