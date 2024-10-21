using UnityEngine;
using UnityEngine.UI;

public class ParallaxUI : MonoBehaviour
{
    private float scrollSpeed = 50f; // Velocidade do movimento
    private RectTransform content; // O container que contém as imagens
    private float contentWidth; // Largura total do conteúdo

    void Start()
    {
        scrollSpeed = Random.Range(50, 100);
        content = GetComponent<RectTransform>();   
        // Calcula a largura total do conteúdo com base no layout e no tamanho dos filhos
        contentWidth = content.rect.width;
    }

    void Update()
    {
        // Move o conteúdo horizontalmente
        content.anchoredPosition += new Vector2(-scrollSpeed * Time.deltaTime, 0);

        // Se o conteúdo saiu completamente da tela, reposiciona no início para um movimento contínuo
        if (content.anchoredPosition.x <= -contentWidth)
        {
            content.anchoredPosition = new Vector2(0, content.anchoredPosition.y);
        }
    }
}
