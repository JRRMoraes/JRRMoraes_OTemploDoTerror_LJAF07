using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


namespace Assets.Scripts.Componentes {

    [RequireComponent(typeof(LivroJogoMotor))]
    public class TelaPanilhaNova : MonoBehaviour {

        LivroJogoMotor livroJogoMotor;

        VisualElement panilhaNova;


        void Start() {
            ///  Iniciar();
        }


        void Update() {

        }


        void Iniciar() {
            livroJogoMotor = GetComponent<LivroJogoMotor>();
            if (!LivroJogoMotor.EhValido(livroJogoMotor))
                return;
            /*           panilhaNova = livroJogoMotor.panilhaNova;
                       if (panilhaNova is null)
                           return;
                       panilhaNova.dataSource = LivroJogo.INSTANCIA.jogoAtual;
                       bool _ativo = (Jogo.EhValido(LivroJogo.INSTANCIA.jogoAtual))
                           && (!Panilha.EhValido(LivroJogo.INSTANCIA.jogoAtual.panilha));
                       if (panilhaNova.enabledSelf != _ativo)
                           panilhaNova.SetEnabled(_ativo);*/
        }
    }
}