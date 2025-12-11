using Assets.Scripts;
using Assets.Scripts.LIB;
using Assets.Scripts.Tipos;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using static Assets.Scripts.Tipos.Conjuntos;


public class TelaHistoria : MonoBehaviour, IPadraoObservador {

	LivroJogoMotor livroJogoMotor;

	VisualElement campanhaTituloVE;

	Label campanhaTitulo;

	VisualElement historiaGroupBox;

	VisualElement comandosGroupBox;

	Button pularHistoriaButton;


	void Awake() {
		livroJogoMotor = GetComponent<LivroJogoMotor>();
		LivroJogo.INSTANCIA.observadoresAlvos.Inscrever(this);
	}


	void Start() {
		AprovarCapturaDeVisualsElements();
	}


	public PaginaExecutora PaginaExecutoraAtual() {
		return LivroJogo.INSTANCIA.paginaExecutora;
	}


	bool AprovarCapturaDeVisualsElements() {
		if (!LivroJogoMotor.EhValido(livroJogoMotor))
			return false;
		if (campanhaTituloVE is null) {
			campanhaTituloVE = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<VisualElement>("CampanhaTituloVE");
			if (campanhaTituloVE is null)
				return false;
		}
		if (campanhaTitulo is null) {
			campanhaTitulo = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<Label>("CampanhaTitulo");
			if (campanhaTitulo is null)
				return false;
		}
		if (historiaGroupBox is null) {
			historiaGroupBox = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<VisualElement>("HistoriaGroupBox");
			if (historiaGroupBox is null)
				return false;
		}
		if (comandosGroupBox is null) {
			comandosGroupBox = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<VisualElement>("ComandosGroupBox");
			if (comandosGroupBox is null)
				return false;
		}
		if (pularHistoriaButton is null) {
			pularHistoriaButton = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<Button>("PularHistoriaButton");
			if (pularHistoriaButton is null)
				return false;
			pularHistoriaButton.RegisterCallback<ClickEvent>(PularHistoria);
		}
		return true;
	}


	public void ImporPropriedadesDosProcessosMotores() {
		if (!PaginaExecutora.EhValido(PaginaExecutoraAtual()))
			return;
		if (PaginaExecutoraAtual().historiaProcesso.monoBehaviour != null)
			return;
		PaginaExecutoraAtual().historiaProcesso.monoBehaviour = this;
		PaginaExecutoraAtual().historiaProcesso.rotinaEhValido = RotinaProcesso_Historia_EhValido;
		PaginaExecutoraAtual().historiaProcesso.rotinasZerado.Add(RotinaProcesso_Historia_Zerado);
		PaginaExecutoraAtual().historiaProcesso.rotinasIniciando.Add(RotinaProcesso_Historia_Iniciando);
		PaginaExecutoraAtual().historiaProcesso.rotinasProcessando.Add(RotinaProcesso_Historia_Processando);
		PaginaExecutoraAtual().historiaProcesso.rotinasConcluindo.Add(RotinaProcesso_Historia_Concluido);
		PaginaExecutoraAtual().historiaProcessoIndice.monoBehaviour = this;
		PaginaExecutoraAtual().historiaProcessoIndice.rotinaEhValido = RotinaProcesso_HistoriaIndice_EhValido;
		PaginaExecutoraAtual().historiaProcessoIndice.rotinasZerado.Add(RotinaProcesso_HistoriaIndice_Zerado);
		PaginaExecutoraAtual().historiaProcessoIndice.rotinasIniciando.Add(RotinaProcesso_HistoriaIndice_Iniciando);
		PaginaExecutoraAtual().historiaProcessoIndice.rotinasProcessando.Add(RotinaProcesso_HistoriaIndice_Processando);
		PaginaExecutoraAtual().historiaProcessoIndice.rotinasConcluindo.Add(RotinaProcesso_HistoriaIndice_Concluido);
		if ((PaginaExecutoraAtual().historiaTextos != null) && (PaginaExecutoraAtual().historiaTextos.Length > 0)) {
			foreach (HistoriaTextoExecucao _historiaTextoI in PaginaExecutoraAtual().historiaTextos) {
				_historiaTextoI.exeProcessoTexto.monoBehaviour = this;
				_historiaTextoI.exeProcessoTexto.rotinaEhValido = RotinaProcesso_HistoriaIndiceTextos_EhValido;
				_historiaTextoI.exeProcessoTexto.rotinasZerado.Add(RotinaProcesso_HistoriaIndiceTextos_Zerado);
				_historiaTextoI.exeProcessoTexto.rotinasIniciando.Add(RotinaProcesso_HistoriaIndiceTextos_Iniciando);
				_historiaTextoI.exeProcessoTexto.rotinasConcluindo.Add(RotinaProcesso_HistoriaIndiceTextos_Concluido);
			}
		}
		if ((PaginaExecutoraAtual().historiaEfeitos != null) && (PaginaExecutoraAtual().historiaEfeitos.Length > 0)) {
			foreach (HistoriaEfeitoExecucao _historiaEfeitoI in PaginaExecutoraAtual().historiaEfeitos) {
				_historiaEfeitoI.exeProcessoEfeito.monoBehaviour = this;
				_historiaEfeitoI.exeProcessoEfeito.rotinaEhValido = RotinaProcesso_HistoriaIndiceEfeitos_EhValido;
				_historiaEfeitoI.exeProcessoEfeito.rotinasZerado.Add(RotinaProcesso_HistoriaIndiceEfeitos_Zerado);
				_historiaEfeitoI.exeProcessoEfeito.rotinasIniciando.Add(RotinaProcesso_HistoriaIndiceEfeitos_Iniciando);
				_historiaEfeitoI.exeProcessoEfeito.rotinasConcluindo.Add(RotinaProcesso_HistoriaIndiceEfeitos_Concluido);
			}
		}
		if ((PaginaExecutoraAtual().historiaImagens != null) && (PaginaExecutoraAtual().historiaImagens.Length > 0)) {
			foreach (HistoriaImagemExecucao _historiaImagemI in PaginaExecutoraAtual().historiaImagens) {
				_historiaImagemI.exeProcessoImagem.monoBehaviour = this;
				_historiaImagemI.exeProcessoImagem.rotinaEhValido = RotinaProcesso_HistoriaIndiceImagens_EhValido;
				_historiaImagemI.exeProcessoImagem.rotinasZerado.Add(RotinaProcesso_HistoriaIndiceImagens_Zerado);
				_historiaImagemI.exeProcessoImagem.rotinasIniciando.Add(RotinaProcesso_HistoriaIndiceImagens_Iniciando);
				_historiaImagemI.exeProcessoImagem.rotinasConcluindo.Add(RotinaProcesso_HistoriaIndiceImagens_Concluido);
			}
		}
	}


	bool LimparTelaHistoria() {
		bool _limpa = (comandosGroupBox != null)
			&& (historiaGroupBox != null);
		if (!_limpa)
			return false;
		_limpa = (!PaginaExecutora.EhValido(PaginaExecutoraAtual()))
			|| (PaginaExecutoraAtual().paginaEstado == PAGINA_EXECUTOR_ESTADO.NULO)
			|| (PaginaExecutoraAtual().paginaEstado == PAGINA_EXECUTOR_ESTADO.INICIALIZADO);
		if (!_limpa)
			return false;
		comandosGroupBox.style.display = Uteis.ObterDisplayStyle(false);
		while (historiaGroupBox.childCount >= 1)
			historiaGroupBox.Remove(historiaGroupBox.Children().First());
		return true;
	}


	public void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao) {
		if (!OBSERVADOR_CONDICAO__JogoAtualEPaginaExecutora.Contains(observadorCondicao))
			return;
		ImporPropriedadesDosProcessosMotores();
		LimparTelaHistoria();
		if (!PaginaExecutora.EhValido(PaginaExecutoraAtual()))
			return;
		if (PaginaExecutoraAtual().historiaProcesso.monoBehaviour is null)
			return;
		PaginaExecutoraAtual().historiaProcesso.Processar();
	}


	bool RotinaProcesso_Historia_EhValido(bool primeiraVez) {
		if (!AprovarCapturaDeVisualsElements())
			return false;
		if (LimparTelaHistoria())
			return false;
		if (!PaginaExecutora.EhValido(PaginaExecutoraAtual()))
			return false;
		if (PaginaExecutoraAtual().paginaEstado != PAGINA_EXECUTOR_ESTADO.HISTORIAS)
			return false;
		if (!Jogo.EhValido(LivroJogo.INSTANCIA.jogoAtual, false))
			return false;

		return true;
	}


	IEnumerator RotinaProcesso_Historia_Zerado(Action<PROCESSO> aoAlterarProcesso) {
		if (PaginaExecutoraAtual().historias != null)
			aoAlterarProcesso?.Invoke(PROCESSO.INICIANDO);
		else
			aoAlterarProcesso?.Invoke(PROCESSO.CONCLUINDO);
		//// LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
		yield return null;
	}


	IEnumerator RotinaProcesso_Historia_Iniciando(Action<PROCESSO> aoAlterarProcesso) {
		aoAlterarProcesso?.Invoke(PROCESSO.INICIANDO_EXEC);
		if (!LivroJogo.INSTANCIA.ehJogoCarregado) {
			livroJogoMotor.historiaVelocidadeDoTexto = livroJogoMotor.historiaVelocidadeNormalDoTexto;
			comandosGroupBox.style.display = Uteis.ObterDisplayStyle(true);
		}
		else {
			livroJogoMotor.historiaVelocidadeDoTexto = Constantes.HISTORIA_VELOCIDADE_TEXTO_RAPIDO;
			comandosGroupBox.style.display = Uteis.ObterDisplayStyle(false);
		}
		PaginaExecutoraAtual().historiaIndice = 0;
		PaginaExecutoraAtual().ImporHistoriaTextosExeProcessoTexto(PROCESSO._ZERADO, false);
		PaginaExecutoraAtual().ImporHistoriaEfeitosExeProcessoEfeito(PROCESSO._ZERADO, false);
		PaginaExecutoraAtual().ImporHistoriaImagensExeProcessoImagem(PROCESSO._ZERADO, false);
		PaginaExecutoraAtual().historiaProcessoIndice.ImporProcesso(PROCESSO._ZERADO);
		aoAlterarProcesso?.Invoke(PROCESSO.PROCESSANDO);
		//// LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
		yield return null;
	}


	IEnumerator RotinaProcesso_Historia_Processando(Action<PROCESSO> aoAlterarProcesso) {
		PaginaExecutoraAtual().historiaProcessoIndice.Processar();
		yield return null;
	}


	IEnumerator RotinaProcesso_Historia_Concluido(Action<PROCESSO> aoAlterarProcesso) {
		comandosGroupBox.style.display = Uteis.ObterDisplayStyle(false);
		PaginaExecutoraAtual().paginaEstado = PAGINA_EXECUTOR_ESTADO.COMBATE;
		aoAlterarProcesso?.Invoke(PROCESSO._FINALIZADO);
		LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
		yield return null;
	}


	bool RotinaProcesso_HistoriaIndice_EhValido(bool primeiraVez) {
		if (PaginaExecutoraAtual().historiaProcesso.processo != PROCESSO.PROCESSANDO)
			return false;
		return true;
	}


	IEnumerator RotinaProcesso_HistoriaIndice_Zerado(Action<PROCESSO> aoAlterarProcesso) {
		PaginaExecutoraAtual().ImporHistoriaTextosExeProcessoTexto(PROCESSO._ZERADO, false);
		PaginaExecutoraAtual().ImporHistoriaEfeitosExeProcessoEfeito(PROCESSO._ZERADO, false);
		PaginaExecutoraAtual().ImporHistoriaImagensExeProcessoImagem(PROCESSO._ZERADO, false);
		aoAlterarProcesso?.Invoke(PROCESSO.INICIANDO);
		//// LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
		yield return null;
	}


	IEnumerator RotinaProcesso_HistoriaIndice_Iniciando(Action<PROCESSO> aoAlterarProcesso) {
		aoAlterarProcesso?.Invoke(PROCESSO.PROCESSANDO);
		//// LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
		yield return null;
	}


	IEnumerator RotinaProcesso_HistoriaIndice_Processando(Action<PROCESSO> aoAlterarProcesso) {
		if (PaginaExecutoraAtual().ObterHistoriaTextosAtuais() != null)
			PaginaExecutoraAtual().ObterHistoriaTextosAtuais().exeProcessoTexto.Processar();
		if (PaginaExecutoraAtual().ObterHistoriaEfeitosAtuais() != null)
			PaginaExecutoraAtual().ObterHistoriaEfeitosAtuais().exeProcessoEfeito.Processar();
		if (PaginaExecutoraAtual().ObterHistoriaImagensAtuais() != null)
			PaginaExecutoraAtual().ObterHistoriaImagensAtuais().exeProcessoImagem.Processar();
		yield return null;
		bool _concluindo = ((PaginaExecutoraAtual().ObterHistoriaTextosAtuais() is null) || (PaginaExecutoraAtual().ObterHistoriaTextosAtuais().exeProcessoTexto.processo == PROCESSO._FINALIZADO))
			&& ((PaginaExecutoraAtual().ObterHistoriaEfeitosAtuais() is null) || (PaginaExecutoraAtual().ObterHistoriaEfeitosAtuais().exeProcessoEfeito.processo == PROCESSO._FINALIZADO))
			&& ((PaginaExecutoraAtual().ObterHistoriaImagensAtuais() is null) || (PaginaExecutoraAtual().ObterHistoriaImagensAtuais().exeProcessoImagem.processo == PROCESSO._FINALIZADO));
		if (_concluindo)
			aoAlterarProcesso?.Invoke(PROCESSO.CONCLUINDO);
	}


	IEnumerator RotinaProcesso_HistoriaIndice_Concluido(Action<PROCESSO> aoAlterarProcesso) {
		aoAlterarProcesso?.Invoke(PROCESSO.CONCLUINDO_EXEC);
		PaginaExecutoraAtual().historiaIndice++;
		if (PaginaExecutoraAtual().ObterHistoriaTextosAtuais() is null) {
			aoAlterarProcesso?.Invoke(PROCESSO._FINALIZADO);
			PaginaExecutoraAtual().historiaProcesso.Processar(PROCESSO.CONCLUINDO);
		}
		else {
			aoAlterarProcesso?.Invoke(PROCESSO._ZERADO);
		}
		yield return null;
	}


	bool RotinaProcesso_HistoriaIndiceTextos_EhValido(bool primeiraVez) {
		if (PaginaExecutoraAtual().historiaProcessoIndice.processo != PROCESSO.PROCESSANDO)
			return false;
		return true;
	}


	IEnumerator RotinaProcesso_HistoriaIndiceTextos_Zerado(Action<PROCESSO> aoAlterarProcesso) {
		aoAlterarProcesso?.Invoke(PROCESSO.INICIANDO);
		yield return PaginaExecutoraAtual().historiaProcessoIndice.ProcessarIEnumerator();
	}


	IEnumerator RotinaProcesso_HistoriaIndiceTextos_Iniciando(Action<PROCESSO> aoAlterarProcesso) {
		aoAlterarProcesso?.Invoke(PROCESSO.PROCESSANDO);
		yield return MontarLabelsHistoriaTextos(aoAlterarProcesso);
		aoAlterarProcesso?.Invoke(PROCESSO.CONCLUINDO);
		yield return PaginaExecutoraAtual().historiaProcessoIndice.ProcessarIEnumerator();
	}


	IEnumerator MontarLabelsHistoriaTextos(Action<PROCESSO> aoAlterarProcesso) {
		bool _mesclaCampanhaTitulo = (campanhaTitulo != null)
			&& (string.IsNullOrWhiteSpace(LivroJogo.INSTANCIA.paginaExecutora.titulo))
			&& (LivroJogo.INSTANCIA.paginaExecutora.idPagina >= 1);
		foreach (string _textosHistoriaI in PaginaExecutoraAtual().ObterHistoriaTextosAtuais().textosHistoria) {
			Label _label = new Label();
			_label.AddToClassList("historiaTexto");
			_label.text = "";
			if (_mesclaCampanhaTitulo) {
				VisualElement _historiaTexto1oVE = new VisualElement();
				_historiaTexto1oVE.AddToClassList("historiaTexto1oVE");
				campanhaTitulo.RemoveFromHierarchy();
				_historiaTexto1oVE.Add(campanhaTitulo);
				_historiaTexto1oVE.Add(_label);
				historiaGroupBox.Add(_historiaTexto1oVE);
				_mesclaCampanhaTitulo = false;
			}
			else {
				historiaGroupBox.Add(_label);
			}
			yield return DatilografarTextoAsync(_label, _textosHistoriaI);
		}
	}


	IEnumerator DatilografarTextoAsync(Label label, string texto) {
		label.text = "";
		bool _rapido = (livroJogoMotor.historiaVelocidadeDoTexto == Constantes.HISTORIA_VELOCIDADE_TEXTO_RAPIDO);
		if (!_rapido) {
			foreach (char _letraI in texto) {
				label.text += _letraI;
				yield return new WaitForSeconds(livroJogoMotor.historiaVelocidadeDoTexto);
				if (livroJogoMotor.historiaVelocidadeDoTexto == Constantes.HISTORIA_VELOCIDADE_TEXTO_RAPIDO) {
					_rapido = true;
					break;
				}
			}
		}
		if (_rapido)
			label.text = texto;
		yield return null;
	}


	IEnumerator RotinaProcesso_HistoriaIndiceTextos_Concluido(Action<PROCESSO> aoAlterarProcesso) {
		aoAlterarProcesso?.Invoke(PROCESSO._FINALIZADO);
		yield return PaginaExecutoraAtual().historiaProcessoIndice.ProcessarIEnumerator();
	}


	bool RotinaProcesso_HistoriaIndiceEfeitos_EhValido(bool primeiraVez) {
		if (PaginaExecutoraAtual().historiaProcessoIndice.processo != PROCESSO.PROCESSANDO)
			return false;
		bool _anterioresFinalizados = ((PaginaExecutoraAtual().ObterHistoriaTextosAtuais() is null) || (PaginaExecutoraAtual().ObterHistoriaTextosAtuais().exeProcessoTexto.processo == PROCESSO._FINALIZADO));
		if (!_anterioresFinalizados)
			return false;
		return true;
	}


	IEnumerator RotinaProcesso_HistoriaIndiceEfeitos_Zerado(Action<PROCESSO> aoAlterarProcesso) {
		aoAlterarProcesso?.Invoke(PROCESSO.INICIANDO);
		yield return PaginaExecutoraAtual().historiaProcessoIndice.ProcessarIEnumerator();
	}


	IEnumerator RotinaProcesso_HistoriaIndiceEfeitos_Iniciando(Action<PROCESSO> aoAlterarProcesso) {
		aoAlterarProcesso?.Invoke(PROCESSO.INICIANDO_EXEC);
		if (!LivroJogo.INSTANCIA.ehJogoCarregado) {
			LivroJogo.INSTANCIA.AdicionarEmJogadorEfeitosAplicados(PaginaExecutoraAtual().ObterHistoriaEfeitosAtuais().efeitos);
			yield return MontarLabelsHistoriaEfeitos(true);
			LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
			yield return new WaitUntil(() => ((LivroJogo.INSTANCIA.jogadorEfeitosAplicados is null)
					|| (LivroJogo.INSTANCIA.jogadorEfeitosAplicados.Length <= 0)));
		}
		else {
			yield return MontarLabelsHistoriaEfeitos(false);
		}
		aoAlterarProcesso?.Invoke(PROCESSO.CONCLUINDO);
		yield return PaginaExecutoraAtual().historiaProcessoIndice.ProcessarIEnumerator();
	}


	IEnumerator MontarLabelsHistoriaEfeitos(bool aguardaAnimacao) {
		foreach (EfeitoExecucao _efeitoI in PaginaExecutoraAtual().ObterHistoriaEfeitosAtuais().efeitos) {
			Label _label = new Label();
			if (_efeitoI.quantidade >= 1)
				_label.AddToClassList("historiaEfeitoBom");
			else
				_label.AddToClassList("historiaEfeitoRuim");
			_label.text = _efeitoI.textoEfeito;
			historiaGroupBox.Add(_label);
		}
		if (aguardaAnimacao)
			yield return new WaitForSeconds(Constantes.TEMPO_ANIMACAO_NORMAL);
		yield return null;
	}


	IEnumerator RotinaProcesso_HistoriaIndiceEfeitos_Concluido(Action<PROCESSO> aoAlterarProcesso) {
		aoAlterarProcesso?.Invoke(PROCESSO._FINALIZADO);
		yield return PaginaExecutoraAtual().historiaProcessoIndice.ProcessarIEnumerator();
	}


	bool RotinaProcesso_HistoriaIndiceImagens_EhValido(bool primeiraVez) {
		if (PaginaExecutoraAtual().historiaProcessoIndice.processo != PROCESSO.PROCESSANDO)
			return false;
		bool _anterioresFinalizados = ((PaginaExecutoraAtual().ObterHistoriaTextosAtuais() is null) || (PaginaExecutoraAtual().ObterHistoriaTextosAtuais().exeProcessoTexto.processo == PROCESSO._FINALIZADO))
			&& ((PaginaExecutoraAtual().ObterHistoriaEfeitosAtuais() is null) || (PaginaExecutoraAtual().ObterHistoriaEfeitosAtuais().exeProcessoEfeito.processo == PROCESSO._FINALIZADO));
		if (!_anterioresFinalizados)
			return false;
		return true;
	}


	IEnumerator RotinaProcesso_HistoriaIndiceImagens_Zerado(Action<PROCESSO> aoAlterarProcesso) {
		aoAlterarProcesso?.Invoke(PROCESSO.INICIANDO);
		yield return PaginaExecutoraAtual().historiaProcessoIndice.ProcessarIEnumerator();
	}


	IEnumerator RotinaProcesso_HistoriaIndiceImagens_Iniciando(Action<PROCESSO> aoAlterarProcesso) {
		aoAlterarProcesso?.Invoke(PROCESSO.PROCESSANDO);
		if (!LivroJogo.INSTANCIA.ehJogoCarregado)
			yield return MontarLabelsHistoriaImagens(true);
		else
			yield return MontarLabelsHistoriaImagens(false);
		aoAlterarProcesso?.Invoke(PROCESSO.CONCLUINDO);
		yield return PaginaExecutoraAtual().historiaProcessoIndice.ProcessarIEnumerator();
	}


	IEnumerator MontarLabelsHistoriaImagens(bool aguardaAnimacao) {
		if (!File.Exists(PaginaExecutoraAtual().ObterHistoriaImagensAtuais().arquivo)) {
			PaginaExecutoraAtual().ObterHistoriaImagensAtuais().exeProcessoImagem.Processar(PROCESSO.CONCLUINDO);
			yield break;
		}
		Task<Texture2D> _textura = Uteis.CarregarImagemAsync(PaginaExecutoraAtual().ObterHistoriaImagensAtuais().arquivo);
		while (!_textura.IsCompleted)
			yield return null;
		if (_textura.Result is null) {
			PaginaExecutoraAtual().ObterHistoriaImagensAtuais().exeProcessoImagem.Processar(PROCESSO.CONCLUINDO);
			yield break;
		}
		Image _imagem = new Image();
		_imagem.image = _textura.Result;
		_imagem.scaleMode = ScaleMode.ScaleToFit;
		//_imagem.style.width = 200;
		//_imagem.style.height = 200;
		_imagem.AddToClassList("historiaImagem");
		historiaGroupBox.Add(_imagem);
		if (aguardaAnimacao)
			yield return new WaitForSeconds(Constantes.TEMPO_ANIMACAO_NORMAL);
		yield return null;
	}


	IEnumerator RotinaProcesso_HistoriaIndiceImagens_Concluido(Action<PROCESSO> aoAlterarProcesso) {
		aoAlterarProcesso?.Invoke(PROCESSO._FINALIZADO);
		yield return PaginaExecutoraAtual().historiaProcessoIndice.ProcessarIEnumerator();
	}


	void PularHistoria(ClickEvent evento) {
		livroJogoMotor.historiaVelocidadeDoTexto = Constantes.HISTORIA_VELOCIDADE_TEXTO_RAPIDO;
		comandosGroupBox.style.display = Uteis.ObterDisplayStyle(false);
	}
}