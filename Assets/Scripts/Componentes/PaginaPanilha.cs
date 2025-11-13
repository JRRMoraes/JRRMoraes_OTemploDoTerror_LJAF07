using UnityEngine;
using UnityEngine.UIElements;


namespace Assets.Scripts.Componentes {

    [RequireComponent(typeof(LivroJogoMotor))]
    public class PaginaPanilha : MonoBehaviour {

        LivroJogoMotor livroJogoMotor;

        VisualElement paginaPanilha;

        VisualElement panilhaNova;

        VisualElement panilhaCompleta;

        VisualElement panilhaMenor;



        void Awake() {
            livroJogoMotor = GetComponent<LivroJogoMotor>();
            if ((livroJogoMotor is null) || (livroJogoMotor.raiz is null))
                return;
            paginaPanilha = livroJogoMotor.raiz.Query<VisualElement>("PaginaPanilha");
            panilhaNova = livroJogoMotor.raiz.Query<VisualElement>("PanilhaNova");
            panilhaCompleta = livroJogoMotor.raiz.Query<VisualElement>("PanilhaCompleta");
            panilhaMenor = livroJogoMotor.raiz.Query<VisualElement>("PanilhaMenor");
        }
    }
}