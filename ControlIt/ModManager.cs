using ColossalFramework.Plugins;
using ColossalFramework.UI;
using System;
using UnityEngine;

namespace ControlIt
{
    public class ModManager : MonoBehaviour
    {
        private bool _initialized;
        private float _timer;

        private UIPanel _menuContainer;
        private UISlicedSprite _centerPart;
        private UIPanel _newsFeedPanel;
        private UIPanel _paradoxAccountPanel;
        private UIPanel _dlcPanelNew;
        private UIScrollablePanel _dlcPanelNewScrollablePanel;
        private UIPanel _workshopAdPanel;
        private UIScrollablePanel _workshopAdPanelScrollablePanel;
        private UILabel _workshopAdPanelDisabledLabel;
        private UISprite _chirper;

        private UIPanel _networkTrafficRestricedPanel;
        private UILabel _networkTrafficRestricedLabel;
        private UILabel _networkTrafficRestricedNumber;

        public void Awake()
        {
            try
            {

            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] ModManager:Awake -> Exception: " + e.Message);
            }
        }

        public void Start()
        {
            try
            {
                if (_menuContainer == null)
                {
                    _menuContainer = GameObject.Find("MenuContainer")?.GetComponent<UIPanel>();
                }

                if (_menuContainer != null && _centerPart == null)
                {
                    _centerPart = _menuContainer.Find("CenterPart")?.GetComponent<UISlicedSprite>();
                }

                if (_menuContainer != null && _newsFeedPanel == null)
                {
                    _newsFeedPanel = _menuContainer.Find("NewsFeedPanel")?.GetComponent<UIPanel>();
                }

                if (_menuContainer != null && _paradoxAccountPanel == null)
                {
                    _paradoxAccountPanel = _menuContainer.Find("ParadoxAccountPanel")?.GetComponent<UIPanel>();
                }

                if (_menuContainer != null && _dlcPanelNew == null)
                {
                    _dlcPanelNew = _menuContainer.Find("DLCPanelNew")?.GetComponent<UIPanel>();

                    if (_dlcPanelNew != null && _dlcPanelNewScrollablePanel == null)
                    {
                        _dlcPanelNewScrollablePanel = _dlcPanelNew.Find("ScrollablePanel")?.GetComponent<UIScrollablePanel>();
                    }
                }

                if (_menuContainer != null && _workshopAdPanel == null)
                {
                    _workshopAdPanel = _menuContainer.Find("WorkshopAdPanel")?.GetComponent<UIPanel>();

                    if (_workshopAdPanel != null && _workshopAdPanelScrollablePanel == null)
                    {
                        _workshopAdPanelScrollablePanel = _workshopAdPanel.Find("Container")?.GetComponent<UIScrollablePanel>();
                    }

                    if (_workshopAdPanel != null && _workshopAdPanelDisabledLabel == null)
                    {
                        _workshopAdPanelDisabledLabel = _workshopAdPanel.Find("DisabledLabel")?.GetComponent<UILabel>();
                    }
                }

                if (_menuContainer != null && _chirper == null)
                {
                    _chirper = _menuContainer.Find("Chirper")?.GetComponent<UISprite>();
                }

                CreateUI();
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] ModManager:Start -> Exception: " + e.Message);
            }
        }

        public void OnDestroy()
        {
            try
            {
                if (_networkTrafficRestricedNumber != null)
                {
                    Destroy(_networkTrafficRestricedNumber.gameObject);
                }

                if (_networkTrafficRestricedLabel != null)
                {
                    Destroy(_networkTrafficRestricedLabel.gameObject);
                }

                if (_networkTrafficRestricedPanel != null)
                {
                    Destroy(_networkTrafficRestricedPanel.gameObject);
                }

                if (_dlcPanelNewScrollablePanel != null)
                {
                    _dlcPanelNewScrollablePanel.isVisible = true;
                }

                if (_workshopAdPanelScrollablePanel != null)
                {
                    _workshopAdPanelScrollablePanel.isVisible = !PluginManager.noWorkshop;
                }

                if (_workshopAdPanelDisabledLabel != null)
                {
                    _workshopAdPanelDisabledLabel.isVisible = PluginManager.noWorkshop;
                }

                if (_centerPart != null)
                {
                    _centerPart.fillAmount = 1f;
                }

                if (_newsFeedPanel != null)
                {
                    _newsFeedPanel.opacity = 1f;
                }

                if (_paradoxAccountPanel != null)
                {
                    _paradoxAccountPanel.opacity = 1f;
                }

                if (_dlcPanelNew != null)
                {
                    _dlcPanelNew.opacity = 1f;
                }

                if (_workshopAdPanel != null)
                {
                    _workshopAdPanel.opacity = 1f;
                }

                if (_chirper != null)
                {
                    _chirper.isVisible = true;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] ModManager:OnDestroy -> Exception: " + e.Message);
            }
        }

        public void Update()
        {
            try
            {
                if (!_initialized || ModConfig.Instance.ConfigUpdated)
                {
                    UpdateUI();

                    _initialized = true;
                    ModConfig.Instance.ConfigUpdated = false;
                }
                else
                {
                    _timer += Time.deltaTime;

                    if (_timer > 5)
                    {
                        _timer -= 5;

                        if (ModConfig.Instance.ShowStatistics)
                        {
                            UpdateStatistics();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] ModManager:Update -> Exception: " + e.Message);
            }
        }

        private void CreateUI()
        {
            try
            {
                _networkTrafficRestricedPanel = UIUtils.CreatePanel("ControlItNetworkTrafficRestricedPanel");
                _networkTrafficRestricedPanel.anchor = UIAnchorStyle.Left | UIAnchorStyle.Bottom;
                _networkTrafficRestricedPanel.width = 500f;
                _networkTrafficRestricedPanel.height = 20f;
                _networkTrafficRestricedPanel.absolutePosition = new Vector3(12f, UIView.GetAView().GetScreenResolution().y - 32f);

                _networkTrafficRestricedLabel = UIUtils.CreateLabel(_networkTrafficRestricedPanel, "NetworkTrafficRestricedLabel", "Restricted in current session: ");
                _networkTrafficRestricedLabel.font = UIUtils.GetUIFont("OpenSans-Semibold");
                _networkTrafficRestricedLabel.autoSize = true;
                _networkTrafficRestricedLabel.height = 18f;
                _networkTrafficRestricedLabel.anchor = UIAnchorStyle.Right | UIAnchorStyle.Top;
                _networkTrafficRestricedLabel.relativePosition = new Vector3(0f, 0f);

                _networkTrafficRestricedNumber = UIUtils.CreateLabel(_networkTrafficRestricedPanel, "NetworkTrafficRestricedNumber", "0 (UGC Details), 0 (Telemetry)");
                _networkTrafficRestricedNumber.font = UIUtils.GetUIFont("OpenSans-Semibold");
                _networkTrafficRestricedNumber.autoSize = true;
                _networkTrafficRestricedNumber.height = 18f;
                _networkTrafficRestricedNumber.anchor = UIAnchorStyle.Right | UIAnchorStyle.Top;
                _networkTrafficRestricedNumber.relativePosition = new Vector3(_networkTrafficRestricedLabel.width + 5f, 0f);

                UpdateUI();

            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] ModManager:CreateUI -> Exception: " + e.Message);
            }
        }

        private void UpdateUI()
        {
            try
            {
                if (_dlcPanelNewScrollablePanel != null)
                {
                    _dlcPanelNewScrollablePanel.isVisible = !ModConfig.Instance.RestrictAdvertising;
                }
                if (_workshopAdPanelScrollablePanel != null)
                {
                    _workshopAdPanelScrollablePanel.isVisible = !ModConfig.Instance.RestrictAdvertising;
                }
                if (_workshopAdPanelDisabledLabel != null)
                {
                    _workshopAdPanelDisabledLabel.isVisible = false;
                }
                if (_centerPart != null)
                {
                    _centerPart.fillAmount = ModConfig.Instance.HideMenuBackground ? 0f : 1f;
                }
                if (_newsFeedPanel != null)
                {
                    _newsFeedPanel.opacity = ModConfig.Instance.NewsPanelOpacity;
                }
                if (_paradoxAccountPanel != null)
                {
                    _paradoxAccountPanel.opacity = ModConfig.Instance.AccountPanelOpacity;
                }
                if (_dlcPanelNew != null)
                {
                    _dlcPanelNew.opacity = ModConfig.Instance.DLCPanelOpacity;
                }
                if (_workshopAdPanel != null)
                {
                    _workshopAdPanel.opacity = ModConfig.Instance.WorkshopPanelOpacity;
                }
                if (_chirper != null)
                {
                    _chirper.isVisible = !ModConfig.Instance.HideChirper;
                }

                if (ModConfig.Instance.ShowStatistics)
                {
                    _networkTrafficRestricedPanel.isVisible = true;
                }
                else
                {
                    _networkTrafficRestricedPanel.isVisible = false;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] ModManager:UpdateUI -> Exception: " + e.Message);
            }
        }

        private void UpdateStatistics()
        {
            try
            {
                _networkTrafficRestricedNumber.text = string.Format("~{0} (UGC Details), ~{1} (Telemetry)", Statistics.Instance.UserGeneratedContentDetailsRequestRestricted.ToString(), Statistics.Instance.TelemetryEntriesSendRestricted.ToString());
            }
            catch (Exception e)
            {
                Debug.Log("[Control It!] ModManager:UpdateStatistics -> Exception: " + e.Message);
            }
        }
    }
}
