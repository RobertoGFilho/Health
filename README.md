# Monitor de Saúde Pessoal

## Descrição
O Monitor de Saúde Pessoal é um aplicativo completo para monitoramento de atividades físicas, saúde e alimentação. O app registra exercícios, sincroniza com APIs de saúde, exibe dados em tempo real e permite que o usuário acompanhe suas métricas ao longo do tempo.

## API Utilizada
- Apple HealthKit para iOS

**Nota:** A integração com a Google Fit API para Android foi descontinuada. O aplicativo atualmente suporta apenas a integração com a Apple HealthKit para dispositivos iOS.

## Requisitos

### Integração com APIs de saúde
- **HealthKit para iOS**: O aplicativo se integra com a Apple HealthKit para coletar dados de atividades físicas e saúde.

### Implementação de gráficos complexos
- **Gráficos de linha e barras**: O aplicativo utiliza gráficos de linha e barras para acompanhar o progresso do usuário ao longo do tempo. Os gráficos são implementados usando a biblioteca `Microcharts`.

### Sincronização de dados entre dispositivos e armazenamento local
- **Sincronização de dados**: O aplicativo sincroniza dados entre dispositivos usando a Apple HealthKit e armazena dados localmente para acesso offline. Isso garante que o usuário possa acessar suas métricas mesmo sem conexão à internet.

### Implementação de funcionalidades nativas avançadas
- **Integração com sensores**: O aplicativo integra-se com sensores como pedômetro e monitor de batimentos cardíacos para coletar dados em tempo real e fornecer informações precisas sobre a saúde do usuário.

### Uso do padrão MVVM com Dependency Injection
- **MVVM**: O aplicativo segue o padrão MVVM (Model-View-ViewModel) para separar a lógica de negócios da interface do usuário, facilitando a manutenção e a escalabilidade do código.
- **Dependency Injection**: O aplicativo utiliza injeção de dependência para gerenciar dependências e promover a reutilização de código.

### Diferenciação avançada de UI e UX entre Android e iOS
- **Human Interface Guidelines para iOS**: O aplicativo segue as diretrizes de Human Interface Guidelines para proporcionar uma experiência de usuário consistente e intuitiva em dispositivos iOS.

### Otimização do aplicativo para performance
- **Threading e tarefas assíncronas**: O aplicativo utiliza threading e tarefas assíncronas para otimizar a performance e garantir uma experiência de usuário fluida.
