using UnityEngine;
using UnityEngine.UIElements;
using Assets.Scripts;

public class MenuInicial : MonoBehaviour {

    public UIDocument uiDocument;

    VisualElement raiz;

    VisualElement buttonJogoSalvo_1;

    VisualElement buttonJogoSalvo_2;

    VisualElement buttonJogoSalvo_3;


    void Start() {
        raiz = uiDocument.rootVisualElement;
        if (raiz is null)
            return;
        raiz.dataSource = LivroJogo.INSTANCIA;
        buttonJogoSalvo_1 = raiz.Query<VisualElement>("ButtonJogoSalvo_1");
        buttonJogoSalvo_2 = raiz.Query<VisualElement>("ButtonJogoSalvo_2");
        buttonJogoSalvo_3 = raiz.Query<VisualElement>("ButtonJogoSalvo_3");
        if (buttonJogoSalvo_1 != null)
            buttonJogoSalvo_1.RegisterCallback<ClickEvent>(LivroJogo.INSTANCIA.SelecionarJogoSalvo_1);
        if (buttonJogoSalvo_2 != null)
            buttonJogoSalvo_2.RegisterCallback<ClickEvent>(LivroJogo.INSTANCIA.SelecionarJogoSalvo_2);
        if (buttonJogoSalvo_3 != null)
            buttonJogoSalvo_3.RegisterCallback<ClickEvent>(LivroJogo.INSTANCIA.SelecionarJogoSalvo_3);
    }
}
