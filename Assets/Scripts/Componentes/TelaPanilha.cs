using UnityEngine;
using UnityEngine.UIElements;


namespace Assets.Scripts.Componentes {

    [RequireComponent(typeof(LivroJogoMotor))]
    public class TelaPanilha : MonoBehaviour {

        LivroJogoMotor livroJogoMotor;

        VisualElement panilha;



        void Start() {
            Iniciar();
        }


        void Update() {

        }


        void Iniciar() {
            livroJogoMotor = GetComponent<LivroJogoMotor>();
            if (livroJogoMotor is null)
                return;
            panilha = livroJogoMotor.panilha;
            if (panilha is null)
                return;
            panilha.dataSource = LivroJogo.INSTANCIA.jogoAtual.panilha;
            bool _ativo = (LivroJogo.INSTANCIA.jogoAtual != null)
                && (LivroJogo.INSTANCIA.jogoAtual.panilha != null);
            if (panilha.enabledSelf != _ativo)
                panilha.SetEnabled(_ativo);
        }
    }
}