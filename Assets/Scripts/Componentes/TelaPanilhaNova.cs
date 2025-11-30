using Assets.Scripts.Tipos;
using System;
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

        PROCESSO processoPanilhaNova = PROCESSO.ZERO;

        string nomeInformado = null;

        JOGO_NIVEL jogoNivelInformado;

        RoladorDeDados roladorDeDados;

        DadosRoladosTotaisParaPanilhaNova dadosRoladosTotaisParaPanilhaNova;

        int rolagensAtributos;

        PROCESSO processoRolagem = PROCESSO.ZERO;



        void Awake() {
            livroJogoMotor = GetComponent<LivroJogoMotor>();
            LivroJogo.INSTANCIA.observadoresAlvos.Inscrever(this);
        }


        public void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao) {
            if (!LivroJogoMotor.EhValido(livroJogoMotor))
                return;
            if (!OBSERVADOR_CONDICAO__JogoAtualEPaginaExecutora.Contains(observadorCondicao))
                return;
            if (panilhaTabView is null) {
                panilhaTabView = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<TabView>("PanilhaTabView");
                if (panilhaTabView is null)
                    return;
            }
            if (!Uteis.TextosIguais(panilhaTabView.activeTab.name, "PanilhaNovaTab", true))
                return;
            if (panilhaNovaTabView is null) {
                panilhaNovaTabView = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<TabView>("PanilhaNovaTabView");
                if (panilhaNovaTabView is null)
                    return;
            }
            if (nomeTab is null) {
                nomeTab = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Tab>("NomeTab");
                if (nomeTab is null)
                    return;
            }
            if (nomeTextField is null) {
                nomeTextField = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<TextField>("NomeTextField");
                if (nomeTextField is null)
                    return;
            }
            if (falhaNomeLabel is null) {
                falhaNomeLabel = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Label>("FalhaNomeLabel");
                if (falhaNomeLabel is null)
                    return;
            }
            if (nivelRadioButtonGroup is null) {
                nivelRadioButtonGroup = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<RadioButtonGroup>("NivelRadioButtonGroup");
                if (nivelRadioButtonGroup is null)
                    return;
            }
            if (falhaNivelLabel is null) {
                falhaNivelLabel = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Label>("FalhaNivelLabel");
                if (falhaNivelLabel is null)
                    return;
            }
            if (salvarNomeENivelButton is null) {
                salvarNomeENivelButton = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Button>("SalvarNomeENivelButton");
                if (salvarNomeENivelButton is null)
                    return;
                salvarNomeENivelButton.RegisterCallback<ClickEvent>(AoSalvarNome);
            }
            if (atributosTab is null) {
                atributosTab = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Tab>("AtributosTab");
                if (atributosTab is null)
                    return;
            }
            if (habilidadeResultadoLabel is null) {
                habilidadeResultadoLabel = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Label>("HabilidadeResultadoLabel");
                if (habilidadeResultadoLabel is null)
                    return;
            }
            if (habilidadeDado1VisualElement is null) {
                habilidadeDado1VisualElement = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<VisualElement>("HabilidadeDado1VisualElement");
                if (habilidadeDado1VisualElement is null)
                    return;
            }
            if (energiaResultadoLabel is null) {
                energiaResultadoLabel = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Label>("EnergiaResultadoLabel");
                if (energiaResultadoLabel is null)
                    return;
            }
            if (energiaDado1VisualElement is null) {
                energiaDado1VisualElement = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<VisualElement>("EnergiaDado1VisualElement");
                if (energiaDado1VisualElement is null)
                    return;
            }
            if (energiaDado2VisualElement is null) {
                energiaDado2VisualElement = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<VisualElement>("EnergiaDado2VisualElement");
                if (energiaDado2VisualElement is null)
                    return;
            }
            if (sorteResultadoLabel is null) {
                sorteResultadoLabel = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Label>("SorteResultadoLabel");
                if (sorteResultadoLabel is null)
                    return;
            }
            if (sorteDado1VisualElement is null) {
                sorteDado1VisualElement = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<VisualElement>("SorteDado1VisualElement");
                if (sorteDado1VisualElement is null)
                    return;
            }
            if (salvarAtributosButton is null) {
                salvarAtributosButton = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Button>("SalvarAtributosButton");
                if (salvarAtributosButton is null)
                    return;
                salvarAtributosButton.RegisterCallback<ClickEvent>(AoSalvarAtributos);
            }
            if (rolarNovamenteButton is null) {
                rolarNovamenteButton = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Button>("RolarNovamenteButton");
                if (rolarNovamenteButton is null)
                    return;
                rolarNovamenteButton.RegisterCallback<ClickEvent>(AoRolarNovamente);
            }

            if (AoNotificar_Informar())
                return;
            if (AoNotificar_ProcessarRolagensDeAtributos())
                return;
        }


        bool AoNotificar_Informar() {
            switch (processoPanilhaNova) {
                case PROCESSO.ZERO:
                    panilhaNovaTabView.activeTab = nomeTab;
                    falhaNomeLabel.style.visibility = Uteis.ObterVisibility(false);
                    processoPanilhaNova = PROCESSO.INICIANDO;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
                    return true;
                case PROCESSO.INICIANDO:
                    if (string.IsNullOrWhiteSpace(nomeInformado))
                        return false;
                    if (nivelRadioButtonGroup.value <= -1)
                        return false;
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
                    processoRolagem = PROCESSO.ZERO;
                    processoPanilhaNova = PROCESSO.PROCESSANDO;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
                    return true;
                case PROCESSO.CONCLUIDO:
                    LivroJogo.INSTANCIA.jogoAtual.panilha = Panilha.CriarPanilhaViaRolagens(dadosRoladosTotaisParaPanilhaNova, nomeInformado, jogoNivelInformado);
                    processoPanilhaNova = PROCESSO.DESTRUIDO;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
                    return true;
            }
            return false;
        }


        void RolarDadosAtributosResultadoFinal(int[] resultados) {
            if (resultados != null) {
                dadosRoladosTotaisParaPanilhaNova.habilidade = resultados[0];
                dadosRoladosTotaisParaPanilhaNova.energia = resultados[1] + resultados[2];
                dadosRoladosTotaisParaPanilhaNova.sorte = resultados[3];
            }
            processoRolagem = PROCESSO.CONCLUIDO;
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
        }


        bool AoNotificar_ProcessarRolagensDeAtributos() {
            if (processoPanilhaNova != PROCESSO.PROCESSANDO)
                return false;
            switch (processoRolagem) {
                case PROCESSO.ZERO:
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
                    return false;
                case PROCESSO.INICIANDO:
                    salvarAtributosButton.SetEnabled(false);
                    rolarNovamenteButton.SetEnabled(false);
                    rolagensAtributos--;
                    dadosRoladosTotaisParaPanilhaNova = new DadosRoladosTotaisParaPanilhaNova();
                    roladorDeDados.Rolar();
                    processoRolagem = PROCESSO.PROCESSANDO;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
                    return true;
                case PROCESSO.CONCLUIDO:
                    dadosRoladosTotaisParaPanilhaNova.habilidade += 6;
                    dadosRoladosTotaisParaPanilhaNova.energia += 12;
                    dadosRoladosTotaisParaPanilhaNova.sorte += 6;
                    habilidadeResultadoLabel.text = dadosRoladosTotaisParaPanilhaNova.habilidade.ToString();
                    energiaResultadoLabel.text = dadosRoladosTotaisParaPanilhaNova.energia.ToString();
                    sorteResultadoLabel.text = dadosRoladosTotaisParaPanilhaNova.sorte.ToString();
                    processoRolagem = PROCESSO.ZERO;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
                    return true;
            }
            return false;
        }


        void AoSalvarNome(ClickEvent evento) {
            string _nome = nomeTextField.value;
            if (string.IsNullOrWhiteSpace(_nome)) {
                falhaNomeLabel.style.visibility = Uteis.ObterVisibility(true);
                nomeTextField.Focus();
                return;
            }
            int _nivel = nivelRadioButtonGroup.value;
            if ((_nivel <= -1) || (_nivel >= 2)) {
                falhaNivelLabel.style.visibility = Uteis.ObterVisibility(true);
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
            processoRolagem = PROCESSO.DESTRUIDO;
            processoPanilhaNova = PROCESSO.CONCLUIDO;
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
        }


        void AoRolarNovamente(ClickEvent evento) {
            processoRolagem = PROCESSO.INICIANDO;
            habilidadeResultadoLabel.text = "?";
            energiaResultadoLabel.text = "?";
            sorteResultadoLabel.text = "?";
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.JOGO_ATUAL);
        }
    }
}
