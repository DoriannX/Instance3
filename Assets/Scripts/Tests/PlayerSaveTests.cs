using NUnit.Framework;
        using UnityEngine;
        
        namespace PlayerTest.Tests
        {
            public class PlayerSaveTests
            {
                private GameObject gameObject;
                private PlayerSave playerSave;
                private MockPlayer mockPlayer;
        
                [SetUp]
                public void Setup()
                {
                    gameObject = new GameObject();
                    playerSave = gameObject.AddComponent<PlayerSave>();
                    mockPlayer = gameObject.AddComponent<MockPlayer>();
                    playerSave.SetPlayer(mockPlayer);
                }
        
                [TearDown]
                public void Teardown()
                {
                    Object.DestroyImmediate(gameObject);
                    PlayerPrefs.DeleteAll();
                }
        
                [Test]
                public void Save_ValidPlayerData_SavesSuccessfully()
                {
                    // Arrange
                    mockPlayer.ammoMultiplier = 1.5f;
                    mockPlayer.cooldownMultiplier = 2.0f;
        
                    // Act
                    bool result = playerSave.Save();
        
                    // Assert
                    Assert.IsTrue(result);
                    Assert.AreEqual(1.5f, PlayerPrefs.GetFloat("player.ammoMultiplier"));
                    Assert.AreEqual(2.0f, PlayerPrefs.GetFloat("player.cooldownMultiplier"));
                }
        
                [Test]
                public void Load_ValidStoredData_LoadsSuccessfully()
                {
                    // Arrange
                    PlayerPrefs.SetFloat("player.ammoMultiplier", 1.5f);
                    PlayerPrefs.SetFloat("player.cooldownMultiplier", 2.0f);
                    mockPlayer.ammoMultiplier = 1.0f;
                    mockPlayer.cooldownMultiplier = 1.0f;
        
                    // Act
                    bool result = playerSave.Load();
        
                    // Assert
                    Assert.IsTrue(result);
                    Assert.AreEqual(1.5f, mockPlayer.ammoMultiplier);
                    Assert.AreEqual(2.0f, mockPlayer.cooldownMultiplier);
                }
        
                [Test]
                public void Load_InvalidStoredData_ReturnsFalse()
                {
                    // Arrange
                    PlayerPrefs.SetFloat("player.ammoMultiplier", -1f);
                    PlayerPrefs.SetFloat("player.cooldownMultiplier", 0f);
                    mockPlayer.ammoMultiplier = 1.0f;
                    mockPlayer.cooldownMultiplier = 1.0f;
        
                    // Act
                    bool result = playerSave.Load();
        
                    // Assert
                    Assert.IsFalse(result);
                    Assert.AreEqual(1.0f, mockPlayer.ammoMultiplier);
                    Assert.AreEqual(1.0f, mockPlayer.cooldownMultiplier);
                }
        
                [Test]
                public void Load_NoStoredData_UsesDefaultValues()
                {
                    // Arrange
                    PlayerPrefs.DeleteAll();
                    mockPlayer.ammoMultiplier = 1.0f;
                    mockPlayer.cooldownMultiplier = 1.0f;
        
                    // Act
                    bool result = playerSave.Load();
        
                    // Assert
                    Assert.IsTrue(result);
                    Assert.AreEqual(1.0f, mockPlayer.ammoMultiplier);
                    Assert.AreEqual(1.0f, mockPlayer.cooldownMultiplier);
                }
        
                [Test]
                public void Awake_MissingPlayerComponent_DisablesComponent()
                {
                    // Arrange
                    var newGameObject = new GameObject();
                    var newPlayerSave = newGameObject.AddComponent<PlayerSave>();
        
                    // Act
                    // Awake is called automatically when component is added
        
                    // Assert
                    Assert.IsFalse(newPlayerSave.enabled);
                    Object.DestroyImmediate(newGameObject);
                }
            }
        
            // Simple mock implementation of Player
            public class MockPlayer : Player
            {
                public float ammoMultiplier;
                public float cooldownMultiplier;
        
                public override float GetAmmoMultiplier() => ammoMultiplier;
                public override float GetCooldownMultiplier() => cooldownMultiplier;
                public override void SetAmmoMultiplier(float value) => ammoMultiplier = value;
                public override void SetCooldownMultiplier(float value) => cooldownMultiplier = value;
            }
        }