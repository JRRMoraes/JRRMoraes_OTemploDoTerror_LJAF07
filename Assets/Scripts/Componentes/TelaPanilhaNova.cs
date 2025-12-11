using Assets.Scripts.LIB;
using Assets.Scripts.Tipos;
using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Assets.Scripts.Tipos.Conjuntos;
using Button = UnityEngine.UIElements.Button;


namespace Assets.Scripts.Componentes {

    [RequireComponent(typeof(LivroJogoMotor))]
    public class TelaPanilhaNova : MonoBehaviour, IPadraoObservador {

        LivroJogoMotor livroJogoMotor;

        TabView panilhaTabView;

        TabView panilhaNovaTabView;

        Tab nomeTab;

        TextField nomeTextField;

        Label falhaNomeLabel;

        RadioButtonGroup nivelRadioButtonGroup;

        Label falhaNivelLabel;

        Button salvarNomeENivelButton;

        Tab atributosTab;

        Label habilidadeResultadoLabel;

        VisualElement habilidadeDado1VisualElement;

        Label energiaResultadoLabel;

        VisualElement energiaDado1VisualElement;

        VisualElement energiaDado2VisualElement;

        Label sorteResultadoLabel;

        VisualElement sorteDado1VisualElement;

        Button salvarAtributosButton;

        Button rolarNovamenteButton;

        ProcessoMotor processoPanilhaNova = new ProcessoMotor();

        string nomeInformado = null;

        JOGO_NIVEL jogoNivelInformado;

        RoladorDeDados roladorDeDados;

        DadosRoladosTotaisParaPanilhaNova dadosRoladosTotaisParaPanilhaNova;

        int rolagensAtributos;

        ProcessoMotor processoRolagem = new ProcessoMotor();



        void Awake() {
            livroJogoMotor = GetComponent<LivroJogoMotor>();
            LivroJogo.INSTANCIA.observadoresAlvos.Inscrever(this);
        }


        public void Start() {
            processoPanilhaNova.monoBehaviour = this;
            processoPanilhaNova.rotinaEhValido = RotinaProcesso_PanilhaNova_EhValido;
            processoPanilhaNova.rotinasZerado.Add(RotinaProcesso_PanilhaNova_Zerado);
            processoPanilhaNova.rotinasIniciando.Add(RotinaProcesso_PanilhaNova_Iniciando);
            processoPanilhaNova.rotinasConcluindo.Add(RotinaProcesso_PanilhaNova_Concluido);
            processoRolagem.monoBehaviour = this;
            processoRolagem.rotinaEhValido = RotinaProcesso_Rolagem_EhValido;
            processoRolagem.rotinasZerado.Add(RotinaProcesso_Rolagem_Zerado);
            processoRolagem.rotinasIniciando.Add(RotinaProcesso_Rolagem_Iniciando);
            processoRolagem.rotinasConcluindo.Add(RotinaProcesso_Rolagem_Concluindo);
        }


        public void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao) {
            if (!OBSERVADOR_CONDICAO__JogoAtualEPaginaExecutora.Contains(observadorCondicao))
                return;
            processoPanilhaNova.Processar();
            processoRolagem.Processar();
        }


        bool RotinaProcesso_PanilhaNova_EhValido(bool primeiraVez) {
            if (!LivroJogoMotor.EhValido(livroJogoMotor))
                return false;
            if (panilhaTabView is null) {
                panilhaTabView = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<TabView>("PanilhaTabView");
                if (panilhaTabView is null)
                    return false;
            }
            if (!Uteis.TextosIguais(panilhaTabView.activeTab.name, "PanilhaNovaTab", true))
                return false;
            if (panilhaNovaTabView is null) {
                panilhaNovaTabView = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<TabView>("PanilhaNovaTabView");
                if (panilhaNovaTabView is null)
                    return false;
            }
            if (nomeTab is null) {
                nomeTab = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Tab>("NomeTab");
                if (nomeTab is null)
                    return false;
            }
            if (nomeTextField is null) {
                nomeTextField = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<TextField>("NomeTextField");
                if (nomeTextField is null)
                    return false;
            }
            if (falhaNomeLabel is null) {
                falhaNomeLabel = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Label>("FalhaNomeLabel");
                if (falhaNomeLabel is null)
                    return false;
            }
            if (nivelRadioButtonGroup is null) {
                nivelRadioButtonGroup = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<RadioButtonGroup>("NivelRadioButtonGroup");
                if (nivelRadioButtonGroup is null)
                    return false;
            }
            if (falhaNivelLabel is null) {
                falhaNivelLabel = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Label>("FalhaNivelLabel");
                if (falhaNivelLabel is null)
                    return false;
            }
            if (salvarNomeENivelButton is null) {
                salvarNomeENivelButton = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Button>("SalvarNomeENivelButton");
                if (salvarNomeENivelButton is null)
                    return false;
                salvarNomeENivelButton.RegisterCallback<ClickEvent>(AoSalvarNome);
            }
            if (atributosTab is null) {
                atributosTab = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Tab>("AtributosTab");
                if (atributosTab is null)
                    return false;
            }
            if (habilidadeResultadoLabel is null) {
                habilidadeResultadoLabel = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Label>("HabilidadeResultadoLabel");
                if (habilidadeResultadoLabel is null)
                    return false;
            }
            if (habilidadeDado1VisualElement is null) {
                habilidadeDado1VisualElement = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<VisualElement>("HabilidadeDado1VisualElement");
                if (habilidadeDado1VisualElement is null)
                    return false;
            }
            if (energiaResultadoLabel is null) {
                energiaResultadoLabel = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Label>("EnergiaResultadoLabel");
                if (energiaResultadoLabel is null)
                    return false;
            }
            if (energiaDado1VisualElement is null) {
                energiaDado1VisualElement = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<VisualElement>("EnergiaDado1VisualElement");
                if (energiaDado1VisualElement is null)
                    return false;
            }
            if (energiaDado2VisualElement is null) {
                energiaDado2VisualElement = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<VisualElement>("EnergiaDado2VisualElement");
                if (energiaDado2VisualElement is null)
                    return false;
            }
            if (sorteResultadoLabel is null) {
                sorteResultadoLabel = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Label>("SorteResultadoLabel");
                if (sorteResultadoLabel is null)
                    return false;
            }
            if (sorteDado1VisualElement is null) {
                sorteDado1VisualElement = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<VisualElement>("SorteDado1VisualElement");
                if (sorteDado1VisualElement is null)
                    return false;
            }
            if (salvarAtributosButton is null) {
                salvarAtributosButton = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Button>("SalvarAtributosButton");
                if (salvarAtributosButton is null)
                    return false;
                salvarAtributosButton.RegisterCallback<ClickEvent>(AoSalvarAtributos);
            }
            if (rolarNovamenteButton is null) {
                rolarNovamenteButton = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Button>("RolarNovamenteButton");
                if (rolarNovamenteButton is null)
                    return false;
                rolarNovamenteButton.RegisterCallback<ClickEvent>(AoRolarNovamente);
            }
            return true;
        }


        IEnumerator RotinaProcesso_PanilhaNova_Zerado(Action<PROCESSO> aoAlterarProcesso) {
            panilhaNovaTabView.activeTab = nomeTab;
            falhaNomeLabel.style.display = Uteis.ObterDisplayStyle(false);
            aoAlterarProcesso?.Invoke(PROCESSO.INICIANDO);
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
            yield return null;
        }


        IEnumerator RotinaProcesso_PanilhaNova_Iniciando(Action<PROCESSO> aoAlterarProcesso) {
            if (string.IsNullOrWhiteSpace(nomeInformado))
                yield break;
            if (nivelRadioButtonGroup.value <= -1)
                yield break;
            aoAlterarProcesso?.Invoke(PROCESSO.INICIANDO_EXEC);
            panilhaNovaTabView.activeTab = atributosTab;
            rolagensAtributos = 3;
            roladorDeDados = new RoladorDeDados();
            roladorDeDados.roladorDeDadosMotor = GetComponent<RoladorDeDadosMotor>();
            roladorDeDados.raizVisualElement = atributosTab;
            roladorDeDados.dadosRolados.Add(new DadoRolado(DADO.D6_AZUL, "HabilidadeDado1VisualElement"));
            roladorDeDados.dadosRolados.Add(new DadoRolado(DADO.D6_VERDE, "EnergiaDado1VisualElement"));
            roladorDeDados.dadosRolados.Add(new DadoRolado(DADO.D6_VERDE, "EnergiaDado2VisualElement"));
            roladorDeDados.dadosRolados.Add(new DadoRolado(DADO.D6_DOURADO, "SorteDado1VisualElement"));
            roladorDeDados.OnResultadoFinal = RolarDadosAtributosResultadoFinal;
            roladorDeDados.Iniciar();
            habilidadeResultadoLabel.text = "?";
            energiaResultadoLabel.text = "?";
            sorteResultadoLabel.text = "?";
            dadosRoladosTotaisParaPanilhaNova = new DadosRoladosTotaisParaPanilhaNova();
            processoRolagem.Processar(PROCESSO._ZERADO);
            aoAlterarProcesso?.Invoke(PROCESSO.PROCESSANDO);
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
            yield return null;
        }


        IEnumerator RotinaProcesso_PanilhaNova_Concluido(Action<PROCESSO> aoAlterarProcesso) {
            LivroJogo.INSTANCIA.jogoAtual.panilha = Panilha.CriarPanilhaViaRolagens(dadosRoladosTotaisParaPanilhaNova, nomeInformado, jogoNivelInformado);
            aoAlterarProcesso?.Invoke(PROCESSO._FINALIZADO);
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
            yield return null;
        }


        void RolarDadosAtributosResultadoFinal(int[] resultados) {
            if (resultados != null) {
                dadosRoladosTotaisParaPanilhaNova.habilidade = resultados[0];
                dadosRoladosTotaisParaPanilhaNova.energia = resultados[1] + resultados[2];
                dadosRoladosTotaisParaPanilhaNova.sorte = resultados[3];
            }
            processoRolagem.Processar(PROCESSO.CONCLUINDO);
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
        }


        bool RotinaProcesso_Rolagem_EhValido(bool primeiraVez) {
            if (!RotinaProcesso_PanilhaNova_EhValido(primeiraVez))
                return false;
            if (processoPanilhaNova.processo != PROCESSO.PROCESSANDO)
                return false;
            return true;
        }


        IEnumerator RotinaProcesso_Rolagem_Zerado(Action<PROCESSO> aoAlterarProcesso) {
            salvarAtributosButton.SetEnabled(true);
            rolarNovamenteButton.SetEnabled(true);
            if (rolagensAtributos == 3) {
                salvarAtributosButton.style.display = Uteis.ObterDisplayStyle(false);
                rolarNovamenteButton.style.display = Uteis.ObterDisplayStyle(true);
                rolarNovamenteButton.text = "Rolar os ATRIBUTOS  ( 3 tentativas )";
            }
            else if (rolagensAtributos == 2) {
                salvarAtributosButton.style.display = Uteis.ObterDisplayStyle(true);
                rolarNovamenteButton.style.display = Uteis.ObterDisplayStyle(true);
                rolarNovamenteButton.text = "Rolar novamente os ATRIBUTOS  ( 2 tentativas )";
            }
            else if (rolagensAtributos == 1) {
                salvarAtributosButton.style.display = Uteis.ObterDisplayStyle(true);
                rolarNovamenteButton.style.display = Uteis.ObterDisplayStyle(true);
                rolarNovamenteButton.text = "Rolar novamente os ATRIBUTOS  ( 1 tentativas )";
            }
            else if (rolagensAtributos <= 0) {
                salvarAtributosButton.style.display = Uteis.ObterDisplayStyle(true);
                salvarAtributosButton.text = "Salvar ATRIBUTOS  ( tentativas acabaram )";
                rolarNovamenteButton.style.display = Uteis.ObterDisplayStyle(false);
            }
            aoAlterarProcesso?.Invoke(PROCESSO.__NULO);
            yield return null;
        }


        IEnumerator RotinaProcesso_Rolagem_Iniciando(Action<PROCESSO> aoAlterarProcesso) {
            aoAlterarProcesso?.Invoke(PROCESSO.INICIANDO_EXEC);
            salvarAtributosButton.SetEnabled(false);
            rolarNovamenteButton.SetEnabled(false);
            rolagensAtributos--;
            dadosRoladosTotaisParaPanilhaNova = new DadosRoladosTotaisParaPanilhaNova();
            roladorDeDados.Rolar();
            aoAlterarProcesso?.Invoke(PROCESSO.PROCESSANDO);
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
            yield return null;
        }


        IEnumerator RotinaProcesso_Rolagem_Concluindo(Action<PROCESSO> aoAlterarProcesso) {
            aoAlterarProcesso?.Invoke(PROCESSO.CONCLUINDO_EXEC);
            dadosRoladosTotaisParaPanilhaNova.habilidade += 6;
            dadosRoladosTotaisParaPanilhaNova.energia += 12;
            dadosRoladosTotaisParaPanilhaNova.sorte += 6;
            habilidadeResultadoLabel.text = dadosRoladosTotaisParaPanilhaNova.habilidade.ToString();
            energiaResultadoLabel.text = dadosRoladosTotaisParaPanilhaNova.energia.ToString();
            sorteResultadoLabel.text = dadosRoladosTotaisParaPanilhaNova.sorte.ToString();
            aoAlterarProcesso?.Invoke(PROCESSO._ZERADO);
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
            yield return null;
        }


        void AoSalvarNome(ClickEvent evento) {
            string _nome = nomeTextField.value;
            if (string.IsNullOrWhiteSpace(_nome)) {
                falhaNomeLabel.style.display = Uteis.ObterDisplayStyle(true);
                nomeTextField.Focus();
                return;
            }
            int _nivel = nivelRadioButtonGroup.value;
            if ((_nivel <= -1) || (_nivel >= 2)) {
                falhaNivelLabel.style.display = Uteis.ObterDisplayStyle(true);
                nivelRadioButtonGroup.Focus();
                return;
            }
            nomeInformado = _nome.Trim();
            jogoNivelInformado = (_nivel == 0) ? JOGO_NIVEL.FACIL : JOGO_NIVEL.NORMAL;
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
        }


        void AoSalvarAtributos(ClickEvent evento) {
            salvarAtributosButton.SetEnabled(false);
            rolarNovamenteButton.SetEnabled(false);
            processoRolagem.Processar(PROCESSO._FINALIZADO);
            processoPanilhaNova.Processar(PROCESSO.CONCLUINDO);
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
        }


        void AoRolarNovamente(ClickEvent evento) {
            habilidadeResultadoLabel.text = "?";
            energiaResultadoLabel.text = "?";
            sorteResultadoLabel.text = "?";
            processoRolagem.Processar(PROCESSO.INICIANDO);
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
        }
    }
}
