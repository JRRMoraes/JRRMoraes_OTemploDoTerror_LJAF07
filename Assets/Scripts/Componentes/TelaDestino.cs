using Assets.Scripts.LIB;
using Assets.Scripts.Tipos;
using System;
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

	[RequireComponent(typeof(LivroJogoMotor))]
	public class TelaDestino : MonoBehaviour, IPadraoObservador {

		LivroJogoMotor livroJogoMotor;

		//[SerializeField] private VisualTreeAsset uxmlTelaDestinoSelecaoButton;
		//private VisualElement raizTelaDestinoSelecaoButton;

		VisualElement telaDestino;

		VisualElement destinoComandosGroupBox;

		VisualElement destinoSelecaoGroupBox;

		Button salvarJogoAtualButton;

		Button curarJogadorButton;

		RoladorDeDados roladorDeDados;


		void Awake() {
			livroJogoMotor = GetComponent<LivroJogoMotor>();
			LivroJogo.INSTANCIA.observadoresAlvos.Inscrever(this);
		}


		void Start() {
			AprovarCapturaDeVisualsElements();
			livroJogoMotor.bookPageCurlMotor.observadoresAoConcluir.Inscrever(this);
		}


		public PaginaExecutora PaginaExecutoraAtual() {
			return LivroJogo.INSTANCIA.paginaExecutora;
		}


		public Jogo JogoAtual() {
			return LivroJogo.INSTANCIA.jogoAtual;
		}


		bool AprovarCapturaDeVisualsElements() {
			if (!LivroJogoMotor.EhValido(livroJogoMotor))
				return false;
			if (destinoComandosGroupBox is null) {
				destinoComandosGroupBox = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<VisualElement>("DestinoComandosGroupBox");
				if (destinoComandosGroupBox is null)
					return false;
			}
			if (salvarJogoAtualButton is null) {
				salvarJogoAtualButton = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<Button>("SalvarJogoAtualButton");
				if (salvarJogoAtualButton is null)
					return false;
				salvarJogoAtualButton.RegisterCallback<ClickEvent>(AoSalvarJogo);
			}
			if (curarJogadorButton is null) {
				curarJogadorButton = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<Button>("CurarJogadorButton");
				if (curarJogadorButton is null)
					return false;
				curarJogadorButton.RegisterCallback<ClickEvent>(AoCurarJogador);
			}
			if (destinoSelecaoGroupBox is null) {
				destinoSelecaoGroupBox = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<VisualElement>("DestinoSelecaoGroupBox");
				if (destinoSelecaoGroupBox is null)
					return false;
			}
			return true;
		}


		public void ImporPropriedadesDosProcessosMotores() {
			if (!PaginaExecutora.EhValido(PaginaExecutoraAtual()))
				return;
			if (PaginaExecutoraAtual().destinoProcesso.monoBehaviour != null)
				return;
			PaginaExecutoraAtual().destinoProcesso.monoBehaviour = this;
			PaginaExecutoraAtual().destinoProcesso.rotinaEhValido = RotinaProcesso_Destino_EhValido;
			PaginaExecutoraAtual().destinoProcesso.rotinasZerado.Add(RotinaProcesso_Destino_Zerado);
			PaginaExecutoraAtual().destinoProcesso.rotinasIniciando.Add(RotinaProcesso_Destino_Iniciando);
			PaginaExecutoraAtual().destinoProcesso.rotinasProcessando.Add(RotinaProcesso_Destino_Processando);
			PaginaExecutoraAtual().destinoProcesso.rotinasConcluindo.Add(RotinaProcesso_Destino_Concluindo);
			PaginaExecutoraAtual().destinoProcessoSalvando.monoBehaviour = this;
			PaginaExecutoraAtual().destinoProcessoSalvando.rotinaEhValido = RotinaProcesso_DestinoSalvando_EhValido;
			PaginaExecutoraAtual().destinoProcessoSalvando.rotinasZerado.Add(RotinaProcesso_DestinoSalvando_Zerado);
			PaginaExecutoraAtual().destinoProcessoSalvando.rotinasIniciando.Add(RotinaProcesso_DestinoSalvando_Iniciando);
			PaginaExecutoraAtual().destinoProcessoSalvando.rotinasConcluindo.Add(RotinaProcesso_DestinoSalvando_Concluindo);
			PaginaExecutoraAtual().destinoProcessoCurando.monoBehaviour = this;
			PaginaExecutoraAtual().destinoProcessoCurando.rotinaEhValido = RotinaProcesso_DestinoCurando_EhValido;
			PaginaExecutoraAtual().destinoProcessoCurando.rotinasZerado.Add(RotinaProcesso_DestinoCurando_Zerado);
			PaginaExecutoraAtual().destinoProcessoCurando.rotinasIniciando.Add(RotinaProcesso_DestinoCurando_Iniciando);
			PaginaExecutoraAtual().destinoProcessoCurando.rotinasConcluindo.Add(RotinaProcesso_DestinoCurando_Concluindo);
			PaginaExecutoraAtual().destinoProcessoRolagem.monoBehaviour = this;
			PaginaExecutoraAtual().destinoProcessoRolagem.rotinaEhValido = RotinaProcesso_DestinoRolagem_EhValido;
			PaginaExecutoraAtual().destinoProcessoRolagem.rotinasIniciando.Add(RotinaProcesso_DestinoRolagem_Iniciando);
			PaginaExecutoraAtual().destinoProcessoRolagem.rotinasConcluindo.Add(RotinaProcesso_DestinoRolagem_Concluindo);
		}


		bool LimparTelaDestino() {
			bool _limpa = (destinoComandosGroupBox != null)
				&& (destinoSelecaoGroupBox != null);
			if (!_limpa)
				return false;
			_limpa = (!PaginaExecutora.EhValido(PaginaExecutoraAtual()))
				|| (PaginaExecutoraAtual().paginaEstado == PAGINA_EXECUTOR_ESTADO.NULO)
				|| (PaginaExecutoraAtual().paginaEstado == PAGINA_EXECUTOR_ESTADO.INICIALIZADO)
				|| (PaginaExecutoraAtual().paginaEstado == PAGINA_EXECUTOR_ESTADO.HISTORIAS)
				|| (PaginaExecutoraAtual().paginaEstado == PAGINA_EXECUTOR_ESTADO.COMBATE);
			if (!_limpa)
				return false;
			destinoComandosGroupBox.style.display = Uteis.ObterDisplayStyle(false);
			while (destinoSelecaoGroupBox.childCount >= 1)
				destinoSelecaoGroupBox.Remove(destinoSelecaoGroupBox.Children().First());
			return true;
		}


		public void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao) {
			if (AoNotificar_ProcessarPassarPaginaDoBook(observadorCondicao))
				return;
			else if (!OBSERVADOR_CONDICAO__JogoAtualEPaginaExecutora.Contains(observadorCondicao))
				return;
			ImporPropriedadesDosProcessosMotores();
			if (LimparTelaDestino())
				return;
			if (!PaginaExecutora.EhValido(PaginaExecutoraAtual()))
				return;
			if ((PaginaExecutoraAtual().destinoProcesso is null) || (PaginaExecutoraAtual().destinoProcesso.monoBehaviour is null))
				return;
			PaginaExecutoraAtual().destinoProcesso.Processar();
			PaginaExecutoraAtual().destinoProcessoSalvando.Processar();
			PaginaExecutoraAtual().destinoProcessoCurando.Processar();
		}


		bool RotinaProcesso_Destino_EhValido(bool primeiraVez) {
			if (!AprovarCapturaDeVisualsElements())
				return false;
			if (LimparTelaDestino())
				return false;
			if (PaginaExecutoraAtual().paginaEstado != PAGINA_EXECUTOR_ESTADO.DESTINOS)
				return false;
			if (!Jogo.EhValido(LivroJogo.INSTANCIA.jogoAtual, false))
				return false;
			return true;
		}


		IEnumerator RotinaProcesso_Destino_Zerado(Action<PROCESSO> aoAlterarProcesso) {
			aoAlterarProcesso?.Invoke(PROCESSO._ZERADO_EXEC);
			if (LivroJogo.INSTANCIA.EhFimDeJogo()) {
				MontarElementosDeFimDeJogo();
				aoAlterarProcesso?.Invoke(PROCESSO.CONCLUINDO);
				LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
				yield break;
			}
			PaginaExecutoraAtual().destinoDesativaBotoes = false;
			PaginaExecutoraAtual().destinoProcessoSalvando.Processar(PROCESSO._ZERADO);
			PaginaExecutoraAtual().destinoProcessoCurando.Processar(PROCESSO._ZERADO);
			if (PaginaExecutoraAtual().destinos != null)
				aoAlterarProcesso?.Invoke(PROCESSO.INICIANDO);
			else
				aoAlterarProcesso?.Invoke(PROCESSO.CONCLUINDO);
			////LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
			yield return null;
		}


		void MontarElementosDeFimDeJogo() {
			destinoComandosGroupBox.style.display = Uteis.ObterDisplayStyle(false);
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


		void AoReiniciarJogo(ClickEvent evento) {
			ProcessarDestinoDesativaBotoes(true);
			LivroJogo.INSTANCIA.ResetarJogo();
			SceneManager.LoadScene("LivroJogo");
		}


		IEnumerator RotinaProcesso_Destino_Iniciando(Action<PROCESSO> aoAlterarProcesso) {
			aoAlterarProcesso?.Invoke(PROCESSO.INICIANDO_EXEC);
			destinoComandosGroupBox.style.display = Uteis.ObterDisplayStyle(!LivroJogo.INSTANCIA.ehJogoCarregado);
			aoAlterarProcesso?.Invoke(PROCESSO.PROCESSANDO);
			MontarElementosDeDestinosSelecoes();
			////LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
			yield return null;
		}


		void MontarElementosDeDestinosSelecoes() {
			foreach (DestinoExecucao _destinoI in PaginaExecutoraAtual().destinoItens) {
				/////raizTelaDestinoSelecaoButton = uxmlTelaDestinoSelecaoButton.Instantiate();
				/////destinoSelecaoGroupBox.rootVisualElement.Add(raizTelaDestinoSelecaoButton);
				Button _destinoSelecaoButton = new Button();
				_destinoI.selecaoButton = _destinoSelecaoButton;
				_destinoSelecaoButton.AddToClassList("destinoSelecao");
				_destinoSelecaoButton.RegisterCallback<ClickEvent>((evento) => AoSelecionarDestino(evento, _destinoI));
				//                desativado ={ AoObterDesativaBotao(destinoI)}
				MontarDestinoSelecaoButtonTextos(_destinoI, _destinoSelecaoButton);
				MontarDestinoSelecaoButtonTesteAtributo(_destinoI, _destinoSelecaoButton);
				if (!string.IsNullOrWhiteSpace(_destinoI.imagemArquivo))
					StartCoroutine(MontarDestinoSelecaoButtonImagem(_destinoI, _destinoSelecaoButton));
				destinoSelecaoGroupBox.Add(_destinoSelecaoButton);
			}
			ProcessarDestinoDesativaBotoes(false);
		}


		void AoSelecionarDestino(ClickEvent evento, DestinoExecucao destinoExecucao) {
			ProcessarDestinoDesativaBotoes(true);
			if (destinoExecucao.testeAtributo == ATRIBUTO_DESTINO_TESTE.NULO) {
				PaginaExecutoraAtual().paginaIdPaginaDestino = destinoExecucao.idPagina;
				PaginaExecutoraAtual().paginaIdCapituloDestino = destinoExecucao.idCapitulo;
				PaginaExecutoraAtual().destinoProcesso.Processar();
				//// LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
			}
			else {
				PaginaExecutoraAtual().destinoRolagemDestino = destinoExecucao;
				PaginaExecutoraAtual().destinoProcessoRolagem.Processar(PROCESSO.INICIANDO);
				//// LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
			}
		}


		IEnumerator RotinaProcesso_Destino_Processando(Action<PROCESSO> aoAlterarProcesso) {
			yield return new WaitWhile(() => (PaginaExecutoraAtual().paginaIdPaginaDestino == PaginaUtils.PAGINA_ZERADA().idPagina)
				&& (PaginaExecutoraAtual().paginaIdCapituloDestino == PaginaUtils.PAGINA_ZERADA().idCapitulo));
			aoAlterarProcesso?.Invoke(PROCESSO.PROCESSANDO_EXEC);
            destinoComandosGroupBox.style.display = Uteis.ObterDisplayStyle(false);
            livroJogoMotor.bookPageCurlMotor.PassarPaginas(JogoAtual().campanhaIdPagina, PaginaExecutoraAtual().paginaIdPaginaDestino);
		}


		IEnumerator RotinaProcesso_Destino_Concluindo(Action<PROCESSO> aoAlterarProcesso) {
			////  yield return new WaitWhile(() => ((PaginaExecutoraAtual().paginaIdPaginaDestino == PaginaUtils.PAGINA_ZERADA().idPagina)
			////       && (PaginaExecutoraAtual().paginaIdCapituloDestino == PaginaUtils.PAGINA_ZERADA().idCapitulo)));
			////            if ((PaginaExecutoraAtual().paginaIdPaginaDestino == PaginaUtils.PAGINA_ZERADA().idPagina) && (PaginaExecutoraAtual().paginaIdCapituloDestino == PaginaUtils.PAGINA_ZERADA().idCapitulo))
			////                return false; 
			aoAlterarProcesso?.Invoke(PROCESSO._FINALIZADO);
			PaginaExecutoraAtual().paginaEstado = PAGINA_EXECUTOR_ESTADO.DESTRUIDO;
			LivroJogo.INSTANCIA.ehJogoCarregado = false;
			LivroJogo.INSTANCIA.ImporCampanhaDestinoNoJogoAtual(PaginaExecutoraAtual().paginaIdPaginaDestino, PaginaExecutoraAtual().paginaIdCapituloDestino);
			LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
			yield return null;
		}


		bool RotinaProcesso_DestinoSalvando_EhValido(bool primeiraVez) {
			if (!RotinaProcesso_Destino_EhValido(primeiraVez))
				return false;
			if (PaginaExecutoraAtual().destinoProcesso.processo != PROCESSO.PROCESSANDO) {
				salvarJogoAtualButton.SetEnabled(true);
				salvarJogoAtualButton.style.display = Uteis.ObterDisplayStyle(false);
				return false;
			}
			return true;
		}


		IEnumerator RotinaProcesso_DestinoSalvando_Zerado(Action<PROCESSO> aoAlterarProcesso) {
			if (LivroJogo.INSTANCIA.ehJogoCarregado) {
				aoAlterarProcesso?.Invoke(PROCESSO._FINALIZADO);
				yield break;
			}
			salvarJogoAtualButton.style.display = Uteis.ObterDisplayStyle(true);
			ProcessarDestinoDesativaBotoes(false);
			yield return null;
		}


		IEnumerator RotinaProcesso_DestinoSalvando_Iniciando(Action<PROCESSO> aoAlterarProcesso) {
			aoAlterarProcesso?.Invoke(PROCESSO.PROCESSANDO);
			//// ProcessarDestinoDesativaBotoes(true);
			//// LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
			LivroJogo.INSTANCIA.SalvarJogoAtualNoJogoSalvo();
			yield return new WaitForSeconds(Constantes.TEMPO_ANIMACAO_NORMAL);
			aoAlterarProcesso?.Invoke(PROCESSO.CONCLUINDO);
			LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
			yield return null;
		}


		IEnumerator RotinaProcesso_DestinoSalvando_Concluindo(Action<PROCESSO> aoAlterarProcesso) {
			aoAlterarProcesso?.Invoke(PROCESSO.CONCLUINDO_EXEC);
			salvarJogoAtualButton.text = "JOGO SALVO !!!";
			aoAlterarProcesso?.Invoke(PROCESSO._FINALIZADO);
			ProcessarDestinoDesativaBotoes(false);
			LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
			yield return null;
		}


		bool RotinaProcesso_DestinoCurando_EhValido(bool primeiraVez) {
			if (!RotinaProcesso_Destino_EhValido(primeiraVez))
				return false;
			if (PaginaExecutoraAtual().destinoProcesso.processo != PROCESSO.PROCESSANDO) {
				curarJogadorButton.SetEnabled(true);
				curarJogadorButton.style.display = Uteis.ObterDisplayStyle(false);
				return false;
			}
			return true;
		}


		IEnumerator RotinaProcesso_DestinoCurando_Zerado(Action<PROCESSO> aoAlterarProcesso) {
			if (LivroJogo.INSTANCIA.jogoAtual.panilha is null) {
				aoAlterarProcesso?.Invoke(PROCESSO._FINALIZADO);
			}
			else if (LivroJogo.INSTANCIA.jogoAtual.panilha.provisao >= 1) {
				curarJogadorButton.text = $"CURAR-SE ?  ( {LivroJogo.INSTANCIA.jogoAtual.panilha.provisao.ToString()} provisões )";
				curarJogadorButton.style.display = Uteis.ObterDisplayStyle(true);
			}
			else {
				curarJogadorButton.text = $"SEM CURA  ( 0 provisões )";
				curarJogadorButton.style.display = Uteis.ObterDisplayStyle(true);
				aoAlterarProcesso?.Invoke(PROCESSO._FINALIZADO);
			}
			ProcessarDestinoDesativaBotoes(false);
			yield return null;
		}


		IEnumerator RotinaProcesso_DestinoCurando_Iniciando(Action<PROCESSO> aoAlterarProcesso) {
			aoAlterarProcesso?.Invoke(PROCESSO.PROCESSANDO);
			//// LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
			////     AplicarCuraEnergiaECustoProvisao();
			yield return new WaitForSeconds(Constantes.TEMPO_ANIMACAO_NORMAL);
			aoAlterarProcesso?.Invoke(PROCESSO.CONCLUINDO);
			LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
			yield return null;
		}


		IEnumerator RotinaProcesso_DestinoCurando_Concluindo(Action<PROCESSO> aoAlterarProcesso) {
			aoAlterarProcesso?.Invoke(PROCESSO._ZERADO);
			ProcessarDestinoDesativaBotoes(false);
			LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
			yield return null;
		}


		bool RotinaProcesso_DestinoRolagem_EhValido(bool primeiraVez) {
			if (!RotinaProcesso_Destino_EhValido(primeiraVez))
				return false;
			if (PaginaExecutoraAtual().destinoProcesso.processo != PROCESSO.PROCESSANDO) {
				salvarJogoAtualButton.SetEnabled(true);
				salvarJogoAtualButton.style.display = Uteis.ObterDisplayStyle(false);
				return false;
			}
			return true;
		}


		IEnumerator RotinaProcesso_DestinoRolagem_Iniciando(Action<PROCESSO> aoAlterarProcesso) {
			aoAlterarProcesso?.Invoke(PROCESSO.INICIANDO_EXEC);
			PaginaExecutoraAtual().destinoRolagemTotal = 0;
			roladorDeDados.Rolar();
			aoAlterarProcesso?.Invoke(PROCESSO.PROCESSANDO);
			LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
			///    StartCoroutine(AguardarProcessoSalvamentoProcessandoParaConcluido());
			yield return null;
		}


		IEnumerator RotinaProcesso_DestinoRolagem_Concluindo(Action<PROCESSO> aoAlterarProcesso) {
			aoAlterarProcesso?.Invoke(PROCESSO.CONCLUINDO_EXEC);
			PaginaExecutoraAtual().destinoRolagemTotal += PaginaExecutoraAtual().destinoRolagemDestino.testeSomarDados;
			bool _teveSorte = false;
			if (PaginaExecutoraAtual().destinoRolagemDestino.testeAtributo == ATRIBUTO_DESTINO_TESTE.HABILIDADE) {
				_teveSorte = (PaginaExecutoraAtual().destinoRolagemTotal <= LivroJogo.INSTANCIA.jogoAtual.panilha.habilidade);
			}
			else if (PaginaExecutoraAtual().destinoRolagemDestino.testeAtributo == ATRIBUTO_DESTINO_TESTE.SORTE) {
				_teveSorte = (PaginaExecutoraAtual().destinoRolagemTotal <= LivroJogo.INSTANCIA.jogoAtual.panilha.sorte);
				/////   AplicarPenalidadeDeTestarSorte();
			}
			int _idPagina = (_teveSorte) ? PaginaExecutoraAtual().destinoRolagemDestino.idPagina : PaginaExecutoraAtual().destinoRolagemDestino.idPaginaAzar;
			PaginaExecutoraAtual().paginaIdPaginaDestino = _idPagina;
			PaginaExecutoraAtual().paginaIdCapituloDestino = PaginaExecutoraAtual().destinoRolagemDestino.idCapitulo;
			aoAlterarProcesso?.Invoke(PROCESSO._FINALIZADO);
			LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
			yield return null;
		}


		bool AoNotificar_ProcessarPassarPaginaDoBook(OBSERVADOR_CONDICAO observadorCondicao) {
			if (observadorCondicao != OBSERVADOR_CONDICAO.PASSAR_PAGINA_DO_BOOK)
				return false;
			if (!PaginaExecutora.EhValido(PaginaExecutoraAtual()))
				return false;
			if (PaginaExecutoraAtual().destinoProcesso.processo != PROCESSO.PROCESSANDO_EXEC)
				return false;
			PaginaExecutoraAtual().destinoProcesso.Processar(PROCESSO.CONCLUINDO);
			LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
			return true;
		}


		void AoSalvarJogo(ClickEvent evento) {
			ProcessarDestinoDesativaBotoes(true);
			PaginaExecutoraAtual().destinoProcessoSalvando.Processar(PROCESSO.INICIANDO);
		}


		void AoCurarJogador(ClickEvent evento) {
			ProcessarDestinoDesativaBotoes(true);
			PaginaExecutoraAtual().destinoProcessoCurando.Processar(PROCESSO.INICIANDO);
		}


		void MontarDestinoSelecaoButtonTextos(DestinoExecucao destinoExecucao, Button destinoSelecaoButton) {
			if ((destinoExecucao.textosDestino != null) && (destinoExecucao.textosDestino.Length >= 1)) {
				foreach (string _textoDestinoI in destinoExecucao.textosDestino) {
					Label _textoDestinoLabel = new Label();
					_textoDestinoLabel.text = _textoDestinoI;
					if (string.IsNullOrWhiteSpace(_textoDestinoLabel.text))
						_textoDestinoLabel.text = $"???  {destinoExecucao.idPagina}  ???";
					_textoDestinoLabel.AddToClassList("destinoTexto");
					destinoSelecaoButton.Add(_textoDestinoLabel);
				}
			}
			else {
				Label _textoDestinoLabel = new Label();
				_textoDestinoLabel.text = destinoExecucao.textoDestino;
				if (string.IsNullOrWhiteSpace(_textoDestinoLabel.text))
					_textoDestinoLabel.text = $"???  {destinoExecucao.idPagina}  ???";
				_textoDestinoLabel.AddToClassList("destinoTexto");
				destinoSelecaoButton.Add(_textoDestinoLabel);
			}
		}


		void MontarDestinoSelecaoButtonTesteAtributo(DestinoExecucao destinoExecucao, Button destinoSelecaoButton) {
			if (destinoExecucao.testeAtributo == ATRIBUTO_DESTINO_TESTE.NULO)
				return;
			roladorDeDados = new RoladorDeDados();
			roladorDeDados.roladorDeDadosMotor = GetComponent<RoladorDeDadosMotor>();
			roladorDeDados.raizVisualElement = destinoSelecaoButton;
			if (destinoExecucao.testeAtributo == ATRIBUTO_DESTINO_TESTE.HABILIDADE) {
				roladorDeDados.dadosRolados.Add(new DadoRolado(DADO.D6_VERDE, "dadoTesteAtributo0"));
				roladorDeDados.dadosRolados.Add(new DadoRolado(DADO.D6_VERDE, "dadoTesteAtributo1"));
			}
			else {
				roladorDeDados.dadosRolados.Add(new DadoRolado(DADO.D6_DOURADO, "dadoTesteAtributo0"));
				roladorDeDados.dadosRolados.Add(new DadoRolado(DADO.D6_DOURADO, "dadoTesteAtributo1"));
			}
			VisualElement _destinoSelecaoAtributo = new VisualElement();
			_destinoSelecaoAtributo.AddToClassList("destinoSelecaoAtributo");
			destinoSelecaoButton.Add(_destinoSelecaoAtributo);
			VisualElement _dadoImagem0 = new VisualElement();
			_dadoImagem0.name = "dadoTesteAtributo0";
			_dadoImagem0.AddToClassList("dadoVE");
			_destinoSelecaoAtributo.Add(_dadoImagem0);
			VisualElement _dadoImagem1 = new VisualElement();
			_dadoImagem1.name = "dadoTesteAtributo1";
			_dadoImagem1.AddToClassList("dadoVE");
			_destinoSelecaoAtributo.Add(_dadoImagem1);
			roladorDeDados.OnResultadoFinal = TesteAtributoResultadoFinal;
			roladorDeDados.Iniciar();
		}


		void TesteAtributoResultadoFinal(int[] resultados) {
			if (resultados != null) {
				foreach (int _itemI in resultados)
					PaginaExecutoraAtual().destinoRolagemTotal += _itemI;
			}
			PaginaExecutoraAtual().destinoProcessoRolagem.Processar(PROCESSO.CONCLUINDO);
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
			_imagem.AddToClassList("destinoImagem");
			destinoSelecaoButton.Add(_imagem);
		}


		void ProcessarDestinoDesativaBotoes(bool destinoDesativaBotoes) {
			PaginaExecutoraAtual().destinoDesativaBotoes = destinoDesativaBotoes;
			if (PaginaExecutoraAtual().destinoDesativaBotoes) {
				salvarJogoAtualButton.SetEnabled(false);
				curarJogadorButton.SetEnabled(false);
				foreach (DestinoExecucao _destinoI in PaginaExecutoraAtual().destinoItens) {
					if (_destinoI.selecaoButton != null)
						_destinoI.selecaoButton.SetEnabled(false);
				}
			}
			else {
				salvarJogoAtualButton.SetEnabled((PaginaExecutoraAtual().destinoProcessoSalvando.processo == PROCESSO._ZERADO));
				curarJogadorButton.SetEnabled((PaginaExecutoraAtual().destinoProcessoCurando.processo == PROCESSO._ZERADO)
					&& (LivroJogo.INSTANCIA.jogoAtual.panilha != null)
					&& (LivroJogo.INSTANCIA.jogoAtual.panilha.provisao >= 1));
				foreach (DestinoExecucao _destinoI in PaginaExecutoraAtual().destinoItens) {
					if (_destinoI.selecaoButton != null)
						_destinoI.selecaoButton.SetEnabled(JogoAtualValidacoes.ValidarAprovacoesDestino(_destinoI.aprovacoes));
				}
			}
		}
	}
}