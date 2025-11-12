using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


namespace Assets.Scripts.Componentes {

    [RequireComponent(typeof(LivroJogoMotor))]
    public class TelaPanilhaNova : MonoBehaviour {

        LivroJogoMotor livroJogoMotor;

        VisualElement panilhaNova;


        void Start() {
            Iniciar();
        }


        void Update() {

        }


        void Iniciar() {
            livroJogoMotor = GetComponent<LivroJogoMotor>();
            if (livroJogoMotor is null)
                return;
            panilhaNova = livroJogoMotor.panilhaNova;
            if (panilhaNova is null)
                return;
            panilhaNova.dataSource = LivroJogo.INSTANCIA.jogoAtual;
            bool _ativo = (LivroJogo.INSTANCIA.jogoAtual != null)
                && (LivroJogo.INSTANCIA.jogoAtual.panilha is null);
            if (panilhaNova.enabledSelf != _ativo)
                panilhaNova.SetEnabled(_ativo);
        }
    }
}