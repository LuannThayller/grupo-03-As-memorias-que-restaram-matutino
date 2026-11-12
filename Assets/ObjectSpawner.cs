
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // ========================================
    // PLAYER
    // ========================================

    [Header("Player")]
    public Transform player;


    // ========================================
    // CONFIGURAÇÕES DOS MOBS
    // ========================================

    [Header("Mob Settings")]

    // Prefabs dos inimigos disponíveis.
    public List<GameObject> mobPrefabs = new();

    // Pontos onde os inimigos podem aparecer.
    public Transform[] mobSpawnPoints;

    // Máximo de inimigos vivos ao mesmo tempo.
    [Min(0)]
    public int maxMobs = 2;

    // Tempo entre tentativas de spawn.
    [Min(0.1f)]
    public float mobSpawnInterval = 5f;

    // Se verdadeiro, gera mobs ao iniciar.
    public bool spawnMobsOnStart = true;

    // Se verdadeiro, repõe mobs mortos.
    public bool respawnMobs = true;


    // ========================================
    // CONFIGURAÇÕES DAS GEMAS
    // ========================================

    [Header("Gem Settings")]

    public List<GameObject> gemPrefabs = new();

    public Transform[] gemSpawnPoints;

    [Min(0)]
    public int maxGems = 3;

    [Min(0.1f)]
    public float gemSpawnInterval = 3f;

    public bool spawnGemsOnStart = true;

    public bool respawnGems = true;


    // ========================================
    // CONTROLE DOS OBJETOS
    // ========================================

    // Cada objeto criado fica associado
    // ao SpawnPoint que está ocupando.
    private Dictionary<Transform, GameObject> activeMobs =
        new();

    private Dictionary<Transform, GameObject> activeGems =
        new();


    // ========================================
    // INICIALIZAÇÃO
    // ========================================

    private void Start()
    {
        // Preenche os pontos até o limite inicial.
        if (spawnMobsOnStart)
        {
            FillSpawnPoints(
                mobPrefabs,
                mobSpawnPoints,
                activeMobs,
                maxMobs,
                true
            );
        }

        if (spawnGemsOnStart)
        {
            FillSpawnPoints(
                gemPrefabs,
                gemSpawnPoints,
                activeGems,
                maxGems,
                false
            );
        }

        // Inicia a reposição automática.
        if (respawnMobs)
            StartCoroutine(MobSpawnRoutine());

        if (respawnGems)
            StartCoroutine(GemSpawnRoutine());
    }


    // ========================================
    // ROTINA DOS MOBS
    // ========================================

    private IEnumerator MobSpawnRoutine()
    {
        while (true)
        {
            // Aguarda antes de tentar repor.
            yield return new WaitForSeconds(
                Mathf.Max(0.1f, mobSpawnInterval)
            );

            // Remove referências de mobs mortos.
            RemoveDestroyedObjects(activeMobs);

            // Cria no máximo um mob por intervalo.
            if (activeMobs.Count < maxMobs)
            {
                SpawnOne(
                    mobPrefabs,
                    mobSpawnPoints,
                    activeMobs,
                    true
                );
            }
        }
    }


    // ========================================
    // ROTINA DAS GEMAS
    // ========================================

    private IEnumerator GemSpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                Mathf.Max(0.1f, gemSpawnInterval)
            );

            // Remove gemas já coletadas.
            RemoveDestroyedObjects(activeGems);

            // Cria uma gema por intervalo.
            if (activeGems.Count < maxGems)
            {
                SpawnOne(
                    gemPrefabs,
                    gemSpawnPoints,
                    activeGems,
                    false
                );
            }
        }
    }


    // ========================================
    // SPAWN INICIAL
    // ========================================

    private void FillSpawnPoints(
        List<GameObject> prefabs,
        Transform[] points,
        Dictionary<Transform, GameObject> active,
        int maximum,
        bool isMob
    )
    {
        // Tenta preencher até o máximo.
        // Para quando não houver pontos livres.
        while (active.Count < maximum)
        {
            bool success = SpawnOne(
                prefabs,
                points,
                active,
                isMob
            );

            if (!success)
                break;
        }
    }


    // ========================================
    // CRIA UM ÚNICO OBJETO
    // ========================================

    private bool SpawnOne(
        List<GameObject> prefabs,
        Transform[] points,
        Dictionary<Transform, GameObject> active,
        bool isMob
    )
    {
        // Verifica se as listas são válidas.
        if (prefabs == null || prefabs.Count == 0 ||
            points == null || points.Length == 0)
        {
            return false;
        }

        // Remove referências destruídas.
        RemoveDestroyedObjects(active);

        // Lista de pontos disponíveis.
        List<Transform> freePoints = new();


        foreach (Transform point in points)
        {
            // Verifica se o SpawnPoint existe.
            if (point == null)
                continue;

            // Verifica se o SpawnPoint está ativo
            // na Hierarchy da Unity.
            //
            // activeInHierarchy considera também
            // se os objetos pais estão desativados.
            //
            // Se o Level_02 estiver desativado,
            // todos os SpawnPoints filhos dele
            // serão ignorados automaticamente.
            if (!point.gameObject.activeInHierarchy)
                continue;

            // Verifica se o SpawnPoint já está ocupado.
            if (active.ContainsKey(point))
                continue;

            // Adiciona apenas pontos ativos e livres.
            freePoints.Add(point);
        }


        // Não existe ponto disponível.
        if (freePoints.Count == 0)
            return false;

        // Remove prefabs nulos da seleção.
        List<GameObject> validPrefabs = new();

        foreach (GameObject prefab in prefabs)
        {
            if (prefab != null)
                validPrefabs.Add(prefab);
        }

        if (validPrefabs.Count == 0)
            return false;

        // Escolhe um prefab aleatório.
        GameObject selectedPrefab = validPrefabs[
            Random.Range(0, validPrefabs.Count)
        ];

        // Escolhe um ponto livre aleatório.
        Transform selectedPoint = freePoints[
            Random.Range(0, freePoints.Count)
        ];

        // Cria exatamente na posição do ponto.
        // Mantém o objeto como filho do Spawner.
        GameObject newObject = Instantiate(
            selectedPrefab,
            selectedPoint.position,
            selectedPrefab.transform.rotation,
            transform
        );

        // Se for inimigo, atribui o Player.
        if (isMob)
        {
            Enemy enemy = newObject.GetComponent<Enemy>();

            if (enemy != null && player != null)
            {
                enemy.player = player;
            }
        }

        // Marca o ponto como ocupado.
        active.Add(selectedPoint, newObject);

        return true;
    }


    // ========================================
    // LIMPEZA DOS OBJETOS DESTRUÍDOS
    // ========================================

    private void RemoveDestroyedObjects(
        Dictionary<Transform, GameObject> active
    )
    {
        List<Transform> pointsToRemove = new();

        foreach (var entry in active)
        {
            // Unity compara objetos destruídos
            // com null.
            if (entry.Value == null)
            {
                pointsToRemove.Add(entry.Key);
            }
        }

        // Libera os pontos para novos spawns.
        foreach (Transform point in pointsToRemove)
        {
            active.Remove(point);
        }
    }


    // ========================================
    // VISUALIZAÇÃO NA SCENE
    // ========================================

    private void OnDrawGizmosSelected()
    {
        // Mobs: vermelho.
        Gizmos.color = Color.red;

        if (mobSpawnPoints != null)
        {
            foreach (Transform point in mobSpawnPoints)
            {
                if (point != null)
                    Gizmos.DrawWireSphere(
                        point.position, 0.3f
                    );
            }
        }

        // Gemas: amarelo.
        Gizmos.color = Color.yellow;

        if (gemSpawnPoints != null)
        {
            foreach (Transform point in gemSpawnPoints)
            {
                if (point != null)
                    Gizmos.DrawWireSphere(
                        point.position, 0.3f
                    );
            }
        }
    }
}
