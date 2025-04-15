#if UNITY_EDITOR
using System;
using System.Collections;
using Item;
using Item.Drops;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tests
{
    public class ItemDropTests
    {
        protected GameObject gameObject;
        protected Player player;
        protected EntityHealth playerHealth;
        protected Collider playerCollider; // Add this field

        [SetUp]
        public void Setup()
        {
            gameObject = new GameObject("TestItem");
            var playerObject = new GameObject("Player");
            player = playerObject.AddComponent<Player>();
            playerHealth = playerObject.AddComponent<EntityHealth>();
            playerCollider = playerObject.AddComponent<SphereCollider>(); // Add collider in setup
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(player.gameObject);
            Object.DestroyImmediate(gameObject);
        }
    }

    public class AmmoTests : ItemDropTests
    {
        private Ammo ammoItem;

        [SetUp]
        public void AmmoSetup()
        {
            ammoItem = gameObject.AddComponent<Ammo>();
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<SphereCollider>();
        }

        [Test]
        public void OnPickUp_WithValidPlayer_SetsPickedUpTrue()
        {
            // Act
            ammoItem.OnPickUp(player);

            // Assert
            Assert.That(ammoItem.GotPickedUp, Is.True);
        }

        [Test]
        public void OnPickUp_WithNullTarget_DoesNotSetPickedUp()
        {
            // Act
            ammoItem.OnPickUp(null);

            // Assert
            Assert.That(ammoItem.GotPickedUp, Is.False);
        }

        [Test]
        public void OnPickUp_CalledTwice_StartsCoroutineOnce()
        {
            // Act
            ammoItem.OnPickUp(player);
            var firstPickup = ammoItem.GotPickedUp;
            ammoItem.OnPickUp(player);

            // Assert
            Assert.That(firstPickup, Is.True);
            Assert.That(ammoItem.GotPickedUp, Is.True);
        }
    }

    public class BandagesTests : ItemDropTests
    {
        private Bandages bandagesItem;

        [SetUp]
        public void BandagesSetup()
        {
            bandagesItem = gameObject.AddComponent<Bandages>();
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<SphereCollider>();
        }

        [Test(ExpectedResult = null)]
        public IEnumerator ApplyEffect_WithValidPlayer_CallsHeal()
        {
            // Arrange
            playerHealth.TakeDamage(playerHealth.Hp - 10); // Leave 10 HP
            bandagesItem.SetHealAmount(20);

            // Wait for damage to apply
            yield return new WaitForEndOfFrame();

            // Store initial health
            int initialHealth = playerHealth.Hp;
            Assert.That(initialHealth, Is.EqualTo(10), "Initial damage not applied correctly");

            // Apply healing effect
            bandagesItem.gameObject.transform.position = player.transform.position;
            bandagesItem.OnPickUp(player);
            yield return new WaitForEndOfFrame();

            // Trigger the heal
            bandagesItem.ApplyEffect();
            yield return new WaitForEndOfFrame();

            // Assert
            Assert.That(playerHealth.Hp, Is.EqualTo(initialHealth + 20), 
                $"Expected health to be {initialHealth + 20} but was {playerHealth.Hp}");
        }

        [Test(ExpectedResult = null)]
        public IEnumerator ApplyEffect_WithInvalidPlayer_ThrowsException()
        {
            // Arrange
            GameObject entityGameObject = new GameObject();
            Entity entity  = entityGameObject.AddComponent<Entity>();
            bandagesItem.OnPickUp(entity);

            yield return new WaitForEndOfFrame();

            // Act & Assert
            var collider = entityGameObject.AddComponent<BoxCollider>();
            Assert.Throws<InvalidCastException>(() => bandagesItem.OnTriggerEnter(collider));
            yield return new WaitForEndOfFrame();
            Object.DestroyImmediate(entityGameObject);
        }
    }

    public class ChipsTests : ItemDropTests
    {
        private Chips chipsItem;

        [SetUp]
        public void ChipsSetup()
        {
            chipsItem = gameObject.AddComponent<Chips>();
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<SphereCollider>();
        }

        [Test(ExpectedResult = null)]
        public IEnumerator ApplyEffect_WithValidPlayer_AddsChips()
        {
            // Arrange
            int initialChips = player.Chips;
            chipsItem.OnPickUp(player);

            // Act
            var collider = player.GetComponent<Collider>();
            chipsItem.OnTriggerEnter(collider);

            // Wait one frame for effect to apply
            yield return null;

            // Assert
            Assert.That(player.Chips, Is.GreaterThan(initialChips));
        }

        [Test]
        public void OnTriggerEnter_WithTriggerCollider_DoesNotSetArrived()
        {
            // Arrange
            var triggerCollider = new GameObject().AddComponent<SphereCollider>();
            triggerCollider.isTrigger = true;

            // Act
            chipsItem.OnTriggerEnter(triggerCollider);

            // Assert
            Assert.That(chipsItem.HasArrived, Is.False);
            
            Object.DestroyImmediate(triggerCollider.gameObject);
        }
    }

    public class PlayerPickupItemTests
    {
        private GameObject playerObject;
        private PlayerPickupItem pickupComponent;
        private GameObject itemObject;
        private ItemDrop testItem;

        [SetUp]
        public void Setup()
        {
            playerObject = new GameObject("Player");
            pickupComponent = playerObject.AddComponent<PlayerPickupItem>();
            playerObject.AddComponent<SphereCollider>();
            playerObject.AddComponent<Player>();

            itemObject = new GameObject("TestItem");
            testItem = itemObject.AddComponent<Ammo>();  // Using Ammo as test item
            itemObject.AddComponent<MeshFilter>();
            itemObject.AddComponent<MeshRenderer>();
            itemObject.AddComponent<SphereCollider>();
        }

        [Test]
        public void OnTriggerEnter_WithValidItem_CallsOnPickup()
        {
            // Arrange
            Assert.That(testItem.GotPickedUp, Is.False);

            // Act
            pickupComponent.OnTriggerEnter(itemObject.GetComponent<Collider>());

            // Assert
            Assert.That(testItem.GotPickedUp, Is.True);
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(itemObject);
            Object.DestroyImmediate(playerObject);
        }
    }
}
#endif