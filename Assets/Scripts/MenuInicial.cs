using Assets.Scripts;
using Assets.Scripts.Tipos;
using UnityEngine;
using UnityEngine.UIElements;
using static Assets.Scripts.Tipos.Conjuntos;


[RequireComponent(typeof(LivroJogoMotor))]
public class MenuInicial : MonoBehaviour, IPadraoObservador {

    LivroJogoMotor livroJogoMotor;

    VisualElement buttonJogoSalvo_1;

    VisualElement buttonJogoSalvo_2;

    VisualElement buttonJogoSalvo_3;


    void Awake() {
        livroJogoMotor = GetComponent<LivroJogoMotor>();
        LivroJogo.INSTANCIA.observadoresAlvos.Inscrever(this);
    }


    public void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao) {
        if (!LivroJogoMotor.EhValido(livroJogoMotor))
            return;
        if (Jogo.EhValido(LivroJogo.INSTANCIA.jogoAtual))
            return;

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
        livroJogoMotor.bookPageCurlMotor.PassarPaginas(0, 1);
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
}