using Assets.Scripts.Tipos;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static Assets.Scripts.Tipos.Conjuntos;
using Button = UnityEngine.UIElements.Button;
using Image = UnityEngine.UIElements.Image;


namespace Assets.Scripts.Componentes {
    public class TelaDestino : MonoBehaviour, IPadraoObservador {

        LivroJogoMotor livroJogoMotor;

        VisualElement destinoComandosGroupBox;

        VisualElement destinoSelecaoGroupBox;


        void Awake() {
            livroJogoMotor = GetComponent<LivroJogoMotor>();
            LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Inscrever(this);
        }


        void OnDestroy() {
            LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Desinscrever(this);
        }


        public PaginaExecutora PaginaExecutoraAtual() {
            return LivroJogo.INSTANCIA.paginaExecutora;
        }


        public Jogo JogoAtual() {
            return LivroJogo.INSTANCIA.jogoAtual;
        }


        public void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao) {
            if (!LivroJogoMotor.EhValido(livroJogoMotor))
                return;
            if (destinoComandosGroupBox is null)
                destinoComandosGroupBox = livroJogoMotor.raiz.Query<VisualElement>("DestinoComandosGroupBox");
            if (destinoSelecaoGroupBox is null)
                destinoSelecaoGroupBox = livroJogoMotor.raiz.Query<VisualElement>("DestinoSelecaoGroupBox");

            AoNotificar_ProcessarTelaDestino(observadorCondicao);
        }


        bool AoNotificar_ProcessarTelaDestino(OBSERVADOR_CONDICAO observadorCondicao) {
            if (!OBSERVADOR_CONDICAO__JogoAtualEPaginaExecutora.Contains(observadorCondicao))
                return false;
            if ((destinoComandosGroupBox is null) || (destinoSelecaoGroupBox is null))
                return false;
            if (!PaginaExecutora.EhValido(PaginaExecutoraAtual())) {
                destinoComandosGroupBox.style.visibility = Uteis.ObterVisibility(false);
                while (destinoSelecaoGroupBox.childCount >= 1)
                    destinoSelecaoGroupBox.Remove(destinoSelecaoGroupBox.Children().First());
                return true;
            }
            if (PaginaExecutoraAtual().paginaEstado != PAGINA_EXECUTOR_ESTADO.DESTINOS)
                return false;
            if (!Jogo.EhValido(LivroJogo.INSTANCIA.jogoAtual))
                return false;

            switch (PaginaExecutoraAtual().destinoProcesso) {
                case PROCESSO.ZERO:
                    if (LivroJogo.INSTANCIA.EhFimDeJogo()) {
                        MontarElementosDeFimDeJogo();
                        PaginaExecutoraAtual().destinoProcesso = PROCESSO.CONCLUIDO;
                        LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                        return true;
                    }
                    if (PaginaExecutoraAtual().destinos != null)
                        PaginaExecutoraAtual().destinoProcesso = PROCESSO.INICIANDO;
                    else
                        PaginaExecutoraAtual().destinoProcesso = PROCESSO.CONCLUIDO;
                    PaginaExecutoraAtual().destinoDesativaBotoes = false;
                    PaginaExecutoraAtual().destinoProcessoSalvando = PROCESSO.ZERO;
                    PaginaExecutoraAtual().destinoProcessoCurando = PROCESSO.ZERO;
                    LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    return true;
                case PROCESSO.INICIANDO:
                    MontarElementosDeDestinosSelecoes();
                    PaginaExecutoraAtual().destinoProcesso = PROCESSO.PROCESSANDO;
                    LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    return true;
                case PROCESSO.PROCESSANDO:
                    if ((PaginaExecutoraAtual().paginaIdPaginaDestino == PaginaUtils.PAGINA_ZERADA().idPagina) && (PaginaExecutoraAtual().paginaIdCapituloDestino == PaginaUtils.PAGINA_ZERADA().idCapitulo))
                        return false;
                    livroJogoMotor.PassarPaginasNoBookAutoFlip(JogoAtual().campanhaIdPagina, PaginaExecutoraAtual().paginaIdPaginaDestino);
                    LivroJogo.INSTANCIA.ImporCampanhaDestinoNoJogoAtual(PaginaExecutoraAtual().paginaIdPaginaDestino, PaginaExecutoraAtual().paginaIdCapituloDestino);
                    PaginaExecutoraAtual().destinoProcesso = PROCESSO.CONCLUIDO;
                    LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    return true;
                case PROCESSO.CONCLUIDO:
                    PaginaExecutoraAtual().destinoProcesso = PROCESSO.DESTRUIDO;
                    PaginaExecutoraAtual().paginaEstado = PAGINA_EXECUTOR_ESTADO.DESTRUIDO;
                    LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    return true;
            }
            return false;
        }


        void AoReiniciarJogo(ClickEvent evento) {
            PaginaExecutoraAtual().destinoDesativaBotoes = true;
            SceneManager.LoadScene("MenuInicial");
        }


        void MontarElementosDeFimDeJogo() {
            destinoComandosGroupBox.style.visibility = Uteis.ObterVisibility(false);
            Label _fimDeJogoLabel = new Label();
            _fimDeJogoLabel.text = "VOCÊ MORREU  -  FIM DE JOGO";
            _fimDeJogoLabel.AddToClassList("destinoTexto");
            _fimDeJogoLabel.AddToClassList("destinoMorte");
            destinoSelecaoGroupBox.Add(_fimDeJogoLabel);
            Button _reiniciarJogoButton = new Button();
            _reiniciarJogoButton.text = "Voltar ao Menu Inicial";
            _reiniciarJogoButton.AddToClassList("destinoSelecao");
            _reiniciarJogoButton.AddToClassList("destinoMorte");
            _reiniciarJogoButton.RegisterCallback<ClickEvent>(AoReiniciarJogo);
            destinoSelecaoGroupBox.Add(_reiniciarJogoButton);
        }


        void MontarElementosDeDestinosSelecoes() {
            foreach (DestinoExecucao _destinoI in PaginaExecutoraAtual().destinoItens) {
                Button _destinoSelecaoButton = new Button();
                _destinoSelecaoButton.AddToClassList("destinoSelecao");
                _destinoSelecaoButton.RegisterCallback<ClickEvent>((evento) => AoSelecionarDestino(evento, _destinoI));
                //                desativado ={ AoObterDesativaBotao(destinoI)}
                MontarDestinoSelecaoButtonTextos(_destinoI, _destinoSelecaoButton);
                MontarDestinoSelecaoButtonTesteSorteHabilidade(_destinoI, _destinoSelecaoButton);
                if (!string.IsNullOrWhiteSpace(_destinoI.imagemArquivo))
                    StartCoroutine(MontarDestinoSelecaoButtonImagem(_destinoI, _destinoSelecaoButton));
                destinoSelecaoGroupBox.Add(_destinoSelecaoButton);
            }
        }


        void AoSelecionarDestino(ClickEvent evento, DestinoExecucao destinoExecucao) {
            //     setDesativaBotoes(true);
            if (destinoExecucao.testeAtributo == ATRIBUTO_DESTINO_TESTE.NULO) {
                PaginaExecutoraAtual().paginaIdPaginaDestino = destinoExecucao.idPagina;
                PaginaExecutoraAtual().paginaIdCapituloDestino = destinoExecucao.idCapitulo;
                LivroJogo.INSTANCIA.ehJogoCarregado = false;
                PaginaExecutoraAtual().destinoProcesso = PROCESSO.PROCESSANDO;
                LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
            }
            else {
                PaginaExecutoraAtual().destinoRolagemTotal = 0;
                PaginaExecutoraAtual().destinoRolagemDestino = destinoExecucao;
                PaginaExecutoraAtual().destinoProcessoRolagem = PROCESSO.INICIANDO;
            }
        }


        void MontarDestinoSelecaoButtonTextos(DestinoExecucao destinoExecucao, Button destinoSelecaoButton) {
            if (!string.IsNullOrWhiteSpace(destinoExecucao.textoDestino)) {
                destinoSelecaoButton.text = destinoExecucao.textoDestino;
            }
            else if ((destinoExecucao.textosDestino != null) && (destinoExecucao.textosDestino.Length >= 1)) {
                foreach (string _textoDestinoI in destinoExecucao.textosDestino) {
                    Label _textoDestinoLabel = new Label();
                    _textoDestinoLabel.text = _textoDestinoI;
                    _textoDestinoLabel.AddToClassList("historiaTexto");
                    destinoSelecaoButton.Add(_textoDestinoLabel);
                }
            }
            else {
                destinoSelecaoButton.text = "???";
            }
        }


        void MontarDestinoSelecaoButtonTesteSorteHabilidade(DestinoExecucao destinoExecucao, Button destinoSelecaoButton) {
            if (destinoExecucao.testeAtributo == ATRIBUTO_DESTINO_TESTE.NULO)
                return;
            //                { MontarRetorno_TesteSorteHabilidade(destinoI)}
        }


        IEnumerator MontarDestinoSelecaoButtonImagem(DestinoExecucao destinoExecucao, Button destinoSelecaoButton) {
            if (string.IsNullOrWhiteSpace(destinoExecucao.imagemArquivo))
                yield break;
            if (!File.Exists(destinoExecucao.imagemArquivo))
                yield break;
            Task<Texture2D> _textura = Uteis.CarregarImagemAsync(destinoExecucao.imagemArquivo);
            while (!_textura.IsCompleted)
                yield return null;
            if (_textura.Result is null)
                yield break;
            Image _imagem = new Image();
            _imagem.image = _textura.Result;
            _imagem.scaleMode = ScaleMode.ScaleToFit;
            //_imagem.style.width = 200;
            //_imagem.style.height = 200;
            _imagem.AddToClassList("historiaImagem");
            destinoSelecaoButton.Add(_imagem);
        }
    }
}