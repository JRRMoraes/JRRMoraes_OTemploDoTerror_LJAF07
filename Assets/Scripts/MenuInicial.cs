using Assets.Scripts;
using Assets.Scripts.Componentes;
using Assets.Scripts.Tipos;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static Assets.Scripts.Tipos.Conjuntos;


[RequireComponent(typeof(LivroJogoMotor))]
public class MenuInicial : MonoBehaviour, IPadraoObservador {

    LivroJogoMotor livroJogoMotor;

    VisualElement paginaPanilha;

    VisualElement paginaCampanha;

    VisualElement buttonJogoSalvo_1;

    VisualElement buttonJogoSalvo_2;

    VisualElement buttonJogoSalvo_3;


    void Awake() {
        livroJogoMotor = GetComponent<LivroJogoMotor>();
        LivroJogo.INSTANCIA.observadoresAlvos.Inscrever(this);
    }


    void OnDestroy() {
        LivroJogo.INSTANCIA.observadoresAlvos.Desinscrever(this);
    }


    public void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao) {
        if (!LivroJogoMotor.EhValido(livroJogoMotor))
            return;
        if (Jogo.EhValido(LivroJogo.INSTANCIA.jogoAtual)) {
            livroJogoMotor.book.OnFlip.RemoveListener(AoPassarPaginaNoMenuInicial);
            return;
        }

        if (paginaPanilha is null)
            paginaPanilha = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<VisualElement>("PaginaPanilha");
        if (paginaCampanha is null)
            paginaCampanha = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<VisualElement>("PaginaCampanha");
        if (buttonJogoSalvo_1 is null) {
            buttonJogoSalvo_1 = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<VisualElement>("ButtonJogoSalvo_1");
            if (buttonJogoSalvo_1 is null)
                return;
            buttonJogoSalvo_1.RegisterCallback<ClickEvent>(SelecionarJogoSalvo_1);
        }
        if (buttonJogoSalvo_2 is null) {
            buttonJogoSalvo_2 = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<VisualElement>("ButtonJogoSalvo_2");
            if (buttonJogoSalvo_2 is null)
                return;
            buttonJogoSalvo_2.RegisterCallback<ClickEvent>(SelecionarJogoSalvo_2);
        }
        if (buttonJogoSalvo_3 is null) {
            buttonJogoSalvo_3 = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<VisualElement>("ButtonJogoSalvo_3");
            if (buttonJogoSalvo_3 is null)
                return;
            buttonJogoSalvo_3.RegisterCallback<ClickEvent>(SelecionarJogoSalvo_3);
        }
    }


    void Interno_SelecionarJogoSalvo(Jogo jogoSelecionado) {
        LivroJogo.INSTANCIA.jogoAtual = jogoSelecionado.Clonar();
        LivroJogo.INSTANCIA.ehJogoCarregado = LivroJogo.INSTANCIA.jogoAtual.AjustarSeForNovoJogo();
        livroJogoMotor.book.OnFlip.AddListener(AoPassarPaginaNoMenuInicial);
        if (paginaPanilha != null)
            paginaPanilha.style.visibility = Uteis.ObterVisibility(false);
        if (paginaCampanha != null)
            paginaCampanha.style.visibility = Uteis.ObterVisibility(false);
        livroJogoMotor.PassarPaginasNoBookAutoFlip(0, 1);
        livroJogoMotor.PassarPaginasNoBookAutoFlip(0, 1);
        livroJogoMotor.PassarPaginasNoBookAutoFlip(0, 1);
    }


    void SelecionarJogoSalvo_1(ClickEvent evento) {
        Interno_SelecionarJogoSalvo(LivroJogo.INSTANCIA.jogoSalvo_1);
    }


    public void SelecionarJogoSalvo_2(ClickEvent evento) {
        Interno_SelecionarJogoSalvo(LivroJogo.INSTANCIA.jogoSalvo_2);
    }


    public void SelecionarJogoSalvo_3(ClickEvent evento) {
        Interno_SelecionarJogoSalvo(LivroJogo.INSTANCIA.jogoSalvo_3);
    }


    void AoPassarPaginaNoMenuInicial() {
        ///// StartCoroutine(AoPassarPaginaNoMenuInicialIEnumerator());
        LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
    }


    IEnumerator AoPassarPaginaNoMenuInicialIEnumerator() {
        yield return null;
        //   yield return new WaitForSeconds(Constantes.TEMPO_ANIMACAO_NORMAL);
    }
}