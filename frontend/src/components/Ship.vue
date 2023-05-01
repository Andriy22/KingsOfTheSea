<template>
  <div :class="`ship-${type}`" class="ship">
    <!-- Render the ship parts -->
    <div v-for="(part, index) in size" :key="index" class="ship-part"></div>
  </div>
</template>

<script>
export default {
  name: "ShipComponent",
  props: {
    type: {
      type: String,
      required: true
    },
    size: {
      type: Number,
      required: true
    }
  }
};
</script>

<style scoped>
.ship {
  display: flex;
}

.ship-part {
  width: 30px;
  height: 30px;
  background-color: navy;
  border: 1px solid white;
}
</style>
