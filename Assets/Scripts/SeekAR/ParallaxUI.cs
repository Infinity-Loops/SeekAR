using UnityEngine;
using UnityEngine.UI;

public class ParallaxUI : MonoBehaviour
{
    private float scrollSpeed = 50f; // Velocidade do movimento
    private RectTransform content; // O container que cont�m as imagens
    private float contentWidth; // Largura total do conte�do

    void Start()
    {
        scrollSpeed = Random.Range(50, 100);
        content = GetComponent<RectTransform>();   
        // Calcula a largura total do conte�do com base no layout e no tamanho dos filhos
        contentWidth = content.rect.width;
    }

    void Update()
    {
        // Move o conte�do horizontalmente
        content.anchoredPosition += new Vector2(-scrollSpeed * Time.deltaTime, 0);

        // Se o conte�do saiu completamente da tela, reposiciona no in�cio para um movimento cont�nuo
        if (content.anchoredPosition.x <= -contentWidth)
        {
            content.anchoredPosition = new Vector2(0, content.anchoredPosition.y);
        }
    }
}
