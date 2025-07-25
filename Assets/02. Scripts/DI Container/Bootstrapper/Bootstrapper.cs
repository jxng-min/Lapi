using TMPro;
using UnityEngine;

public abstract class Bootstrapper : MonoBehaviour
{
    [Header("Installer's Root")]
    [SerializeField] private Transform m_installer_root;

    private IInstaller[] m_installers;

    private void Awake()
    {
        m_installers = m_installer_root.GetComponentsInChildren<IInstaller>();
    }

    private void Start()
    {
        foreach (var installer in m_installers)
        {
            installer.Install();
        }
    }
}
