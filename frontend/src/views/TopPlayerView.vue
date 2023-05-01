<template>
  <v-card
    class="mx-auto"
    max-width="1000"
  >
    <v-card-item>
    </v-card-item>

    <v-card-text class="py-0">
      <v-col
        class="text-h2"
        cols="12"
      >
        Рейтинг топ 10 гравців
      </v-col>

    </v-card-text>

    <v-table fixed-header>
      <thead>
      <tr>
        <th class="text-left">
          Гравець
        </th>
        <th class="text-left">
          Рейтинг ELO
        </th>
      </tr>
      </thead>
      <tbody>
      <tr
        v-for="item in $store.state.account.requests.top.data"
        :key="item.nickName"
      >
        <td>{{ item.nickName }}</td>
        <td>
          {{ item.eloRating }}
        </td>
      </tr>
      </tbody>
    </v-table>
  </v-card>
</template>

<script>


export default ({
  data: () => ({}),
  mounted() {
    this.$store.dispatch("account/getTopPlayers");
  }
});
</script>
